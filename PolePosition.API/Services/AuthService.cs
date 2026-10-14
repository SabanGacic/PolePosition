using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PolePosition.API.Common;
using PolePosition.API.Data;
using PolePosition.API.DTOs;
using PolePosition.API.Models;

namespace PolePosition.API.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task LogoutAsync();
}

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICurrentUserService _currentUser;
    private readonly ApplicationDbContext _context;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IJwtTokenService jwtTokenService,
        ICurrentUserService currentUser,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
        _currentUser = currentUser;
        _context = context;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var email = dto.Email.Trim();

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim()
        };

        // Several SaveChanges calls happen inside UserManager, so we wrap them in one transaction
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var createResult = await _userManager.CreateAsync(user, dto.Password);
        if (!createResult.Succeeded)
        {
            throw new BusinessException(MapIdentityErrors(createResult));
        }

        // Role is never taken from the client: every new user is a Driver
        var roleResult = await _userManager.AddToRoleAsync(user, Roles.Driver);
        if (!roleResult.Succeeded)
        {
            throw new BusinessException(MapIdentityErrors(roleResult));
        }

        await transaction.CommitAsync();

        return await BuildResponseAsync(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email.Trim());

        if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            throw new UnauthorizedException("Neispravan e-mail ili lozinka.");
        }

        return await BuildResponseAsync(user);
    }

    public async Task LogoutAsync()
    {
        var now = DateTime.UtcNow;

        _context.RevokedTokens.Add(new RevokedToken
        {
            Jti = _currentUser.Jti,
            ExpiresAt = _currentUser.TokenExpiresAt,
            RevokedAt = now
        });
        await _context.SaveChangesAsync();

        // Expired tokens are invalid anyway, so their revocation records can be removed
        await _context.RevokedTokens.Where(t => t.ExpiresAt < now).ExecuteDeleteAsync();
    }

    private async Task<AuthResponseDto> BuildResponseAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var (token, expiresAt) = _jwtTokenService.CreateToken(user, roles);

        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = roles.ToList()
        };
    }

    private static string MapIdentityErrors(IdentityResult result)
    {
        var codes = result.Errors.Select(e => e.Code).ToList();

        if (codes.Contains("DuplicateUserName") || codes.Contains("DuplicateEmail"))
        {
            return "Korisnik s ovom e-mail adresom već postoji.";
        }

        if (codes.Any(c => c.StartsWith("Password")))
        {
            return "Lozinka mora imati najmanje 8 znakova, uključujući veliko slovo, malo slovo i broj.";
        }

        return string.Join(" ", result.Errors.Select(e => e.Description));
    }
}