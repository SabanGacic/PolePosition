using Microsoft.EntityFrameworkCore;
using PolePosition.API.Common;
using PolePosition.API.Data;
using PolePosition.API.DTOs;
using PolePosition.API.Models;

namespace PolePosition.API.Services;

public class TrackService : ITrackService
{
    private readonly ApplicationDbContext _context;

    public TrackService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<TrackDto>> GetAsync(TrackSearchQuery query)
    {
        var tracks = _context.Tracks.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            tracks = tracks.Where(t => t.Name.Contains(query.Name));
        }

        var totalCount = await tracks.CountAsync();

        var items = await tracks
            .OrderBy(t => t.Name)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(t => new TrackDto
            {
                Id = t.Id,
                Name = t.Name,
                Location = t.Location,
                Description = t.Description,
                LengthMeters = t.LengthMeters
            })
            .ToListAsync();

        return new PagedResult<TrackDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<TrackDto> GetByIdAsync(int id)
    {
        var track = await _context.Tracks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new NotFoundException("Staza nije pronađena.");

        return ToDto(track);
    }

    public async Task<TrackDto> CreateAsync(TrackUpsertDto dto)
    {
        await EnsureNameIsUniqueAsync(dto.Name, null);

        var track = new Track
        {
            Name = dto.Name.Trim(),
            Location = dto.Location.Trim(),
            Description = dto.Description?.Trim(),
            LengthMeters = dto.LengthMeters
        };

        _context.Tracks.Add(track);
        await _context.SaveChangesAsync();

        return ToDto(track);
    }

    public async Task<TrackDto> UpdateAsync(int id, TrackUpsertDto dto)
    {
        var track = await _context.Tracks.FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new NotFoundException("Staza nije pronađena.");

        await EnsureNameIsUniqueAsync(dto.Name, id);

        track.Name = dto.Name.Trim();
        track.Location = dto.Location.Trim();
        track.Description = dto.Description?.Trim();
        track.LengthMeters = dto.LengthMeters;

        await _context.SaveChangesAsync();

        return ToDto(track);
    }

    public async Task DeleteAsync(int id)
    {
        var track = await _context.Tracks.FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new NotFoundException("Staza nije pronađena.");

        // Deletion is blocked when other records depend on the track
        var isUsed = await _context.Karts.AnyAsync(k => k.TrackId == id)
            || await _context.Sessions.AnyAsync(s => s.TrackId == id);

        if (isUsed)
        {
            throw new BusinessException("Stazu nije moguće obrisati jer je povezana s kartovima ili terminima.");
        }

        _context.Tracks.Remove(track);
        await _context.SaveChangesAsync();
    }

    private async Task EnsureNameIsUniqueAsync(string name, int? excludeId)
    {
        var normalized = name.Trim();
        var exists = await _context.Tracks
            .AnyAsync(t => t.Name == normalized && (excludeId == null || t.Id != excludeId));

        if (exists)
        {
            throw new BusinessException("Staza s tim nazivom već postoji.");
        }
    }

    private static TrackDto ToDto(Track track) => new()
    {
        Id = track.Id,
        Name = track.Name,
        Location = track.Location,
        Description = track.Description,
        LengthMeters = track.LengthMeters
    };
}