using PolePosition.API.Common;
using PolePosition.API.DTOs;

namespace PolePosition.API.Services;

public interface ITrackService
{
    Task<PagedResult<TrackDto>> GetAsync(TrackSearchQuery query);
    Task<TrackDto> GetByIdAsync(int id);
    Task<TrackDto> CreateAsync(TrackUpsertDto dto);
    Task<TrackDto> UpdateAsync(int id, TrackUpsertDto dto);
    Task DeleteAsync(int id);
}