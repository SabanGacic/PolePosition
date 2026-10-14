using Microsoft.EntityFrameworkCore;
using PolePosition.API.Common;
using PolePosition.API.Data;
using PolePosition.API.DTOs;
using PolePosition.API.Models;

namespace PolePosition.API.Services;

public interface ISessionTypeService
{
    Task<PagedResult<SessionTypeDto>> GetAsync(SessionTypeSearchQuery query);
    Task<SessionTypeDto> GetByIdAsync(int id);
    Task<SessionTypeDto> CreateAsync(SessionTypeUpsertDto dto);
    Task<SessionTypeDto> UpdateAsync(int id, SessionTypeUpsertDto dto);
    Task DeleteAsync(int id);
}

public class SessionTypeService : ISessionTypeService
{
    private readonly ApplicationDbContext _context;

    public SessionTypeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<SessionTypeDto>> GetAsync(SessionTypeSearchQuery query)
    {
        var sessionTypes = _context.SessionTypes.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            sessionTypes = sessionTypes.Where(s => s.Name.Contains(query.Name));
        }

        var totalCount = await sessionTypes.CountAsync();

        var items = await sessionTypes
            .OrderBy(s => s.Name)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(s => new SessionTypeDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description
            })
            .ToListAsync();

        return new PagedResult<SessionTypeDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<SessionTypeDto> GetByIdAsync(int id)
    {
        var sessionType = await _context.SessionTypes.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id)
            ?? throw new NotFoundException("Tip sesije nije pronađen.");

        return ToDto(sessionType);
    }

    public async Task<SessionTypeDto> CreateAsync(SessionTypeUpsertDto dto)
    {
        await EnsureNameIsUniqueAsync(dto.Name, null);

        var sessionType = new SessionType
        {
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim()
        };

        _context.SessionTypes.Add(sessionType);
        await _context.SaveChangesAsync();

        return ToDto(sessionType);
    }

    public async Task<SessionTypeDto> UpdateAsync(int id, SessionTypeUpsertDto dto)
    {
        var sessionType = await _context.SessionTypes.FirstOrDefaultAsync(s => s.Id == id)
            ?? throw new NotFoundException("Tip sesije nije pronađen.");

        await EnsureNameIsUniqueAsync(dto.Name, id);

        sessionType.Name = dto.Name.Trim();
        sessionType.Description = dto.Description?.Trim();

        await _context.SaveChangesAsync();

        return ToDto(sessionType);
    }

    public async Task DeleteAsync(int id)
    {
        var sessionType = await _context.SessionTypes.FirstOrDefaultAsync(s => s.Id == id)
            ?? throw new NotFoundException("Tip sesije nije pronađen.");

        var isUsed = await _context.Sessions.AnyAsync(s => s.SessionTypeId == id);

        if (isUsed)
        {
            throw new BusinessException("Tip sesije nije moguće obrisati jer postoje termini s tim tipom.");
        }

        _context.SessionTypes.Remove(sessionType);
        await _context.SaveChangesAsync();
    }

    private async Task EnsureNameIsUniqueAsync(string name, int? excludeId)
    {
        var normalized = name.Trim();
        var exists = await _context.SessionTypes
            .AnyAsync(s => s.Name == normalized && (excludeId == null || s.Id != excludeId));

        if (exists)
        {
            throw new BusinessException("Tip sesije s tim nazivom već postoji.");
        }
    }

    private static SessionTypeDto ToDto(SessionType sessionType) => new()
    {
        Id = sessionType.Id,
        Name = sessionType.Name,
        Description = sessionType.Description
    };
}