using Microsoft.EntityFrameworkCore;
using PolePosition.API.Common;
using PolePosition.API.Data;
using PolePosition.API.DTOs;
using PolePosition.API.Models;

namespace PolePosition.API.Services;

public interface IEquipmentTypeService
{
    Task<PagedResult<EquipmentTypeDto>> GetAsync(EquipmentTypeSearchQuery query);
    Task<EquipmentTypeDto> GetByIdAsync(int id);
    Task<EquipmentTypeDto> CreateAsync(EquipmentTypeUpsertDto dto);
    Task<EquipmentTypeDto> UpdateAsync(int id, EquipmentTypeUpsertDto dto);
    Task DeleteAsync(int id);
}

public class EquipmentTypeService : IEquipmentTypeService
{
    private readonly ApplicationDbContext _context;

    public EquipmentTypeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<EquipmentTypeDto>> GetAsync(EquipmentTypeSearchQuery query)
    {
        var equipmentTypes = _context.EquipmentTypes.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            equipmentTypes = equipmentTypes.Where(e => e.Name.Contains(query.Name));
        }

        var totalCount = await equipmentTypes.CountAsync();

        var items = await equipmentTypes
            .OrderBy(e => e.Name)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(e => new EquipmentTypeDto
            {
                Id = e.Id,
                Name = e.Name,
                AdditionalPrice = e.AdditionalPrice
            })
            .ToListAsync();

        return new PagedResult<EquipmentTypeDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<EquipmentTypeDto> GetByIdAsync(int id)
    {
        var equipmentType = await _context.EquipmentTypes.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id)
            ?? throw new NotFoundException("Tip opreme nije pronađen.");

        return ToDto(equipmentType);
    }

    public async Task<EquipmentTypeDto> CreateAsync(EquipmentTypeUpsertDto dto)
    {
        await EnsureNameIsUniqueAsync(dto.Name, null);

        var equipmentType = new EquipmentType
        {
            Name = dto.Name.Trim(),
            AdditionalPrice = dto.AdditionalPrice
        };

        _context.EquipmentTypes.Add(equipmentType);
        await _context.SaveChangesAsync();

        return ToDto(equipmentType);
    }

    public async Task<EquipmentTypeDto> UpdateAsync(int id, EquipmentTypeUpsertDto dto)
    {
        var equipmentType = await _context.EquipmentTypes.FirstOrDefaultAsync(e => e.Id == id)
            ?? throw new NotFoundException("Tip opreme nije pronađen.");

        await EnsureNameIsUniqueAsync(dto.Name, id);

        equipmentType.Name = dto.Name.Trim();
        equipmentType.AdditionalPrice = dto.AdditionalPrice;

        await _context.SaveChangesAsync();

        return ToDto(equipmentType);
    }

    public async Task DeleteAsync(int id)
    {
        var equipmentType = await _context.EquipmentTypes.FirstOrDefaultAsync(e => e.Id == id)
            ?? throw new NotFoundException("Tip opreme nije pronađen.");

        var isUsed = await _context.ReservationEquipments.AnyAsync(r => r.EquipmentTypeId == id);

        if (isUsed)
        {
            throw new BusinessException("Tip opreme nije moguće obrisati jer je dodan u postojeće rezervacije.");
        }

        _context.EquipmentTypes.Remove(equipmentType);
        await _context.SaveChangesAsync();
    }

    private async Task EnsureNameIsUniqueAsync(string name, int? excludeId)
    {
        var normalized = name.Trim();
        var exists = await _context.EquipmentTypes
            .AnyAsync(e => e.Name == normalized && (excludeId == null || e.Id != excludeId));

        if (exists)
        {
            throw new BusinessException("Tip opreme s tim nazivom već postoji.");
        }
    }

    private static EquipmentTypeDto ToDto(EquipmentType equipmentType) => new()
    {
        Id = equipmentType.Id,
        Name = equipmentType.Name,
        AdditionalPrice = equipmentType.AdditionalPrice
    };
}