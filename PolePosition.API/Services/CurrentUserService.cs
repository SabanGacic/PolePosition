using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using PolePosition.API.Common;

namespace PolePosition.API.Services;

public interface ICurrentUserService
{
    string UserId { get; }
    string Jti { get; }
    DateTime TokenExpiresAt { get; }
}

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _accessor;

    public CurrentUserService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    private ClaimsPrincipal? Principal => _accessor.HttpContext?.User;

    public string UserId => Principal?.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedException("Prijava je obavezna.");

    public string Jti => Principal?.FindFirstValue(JwtRegisteredClaimNames.Jti)
        ?? throw new UnauthorizedException("Prijava je obavezna.");

    public DateTime TokenExpiresAt
    {
        get
        {
            var exp = Principal?.FindFirstValue(JwtRegisteredClaimNames.Exp)
                ?? throw new UnauthorizedException("Prijava je obavezna.");
            return DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp)).UtcDateTime;
        }
    }
}