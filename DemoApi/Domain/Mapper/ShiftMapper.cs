using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;

namespace DemoApi.Domain.Mapper
{
    public class ShiftMapper
    {
        public static ShiftViewModel MapToViewModel(Shift entity) => new()
        {
            Id = entity.Id,
            FacilityId = entity.FacilityId,
            Name = entity.Name,
            StartTime = entity.StartTime,
            EndTime = entity.EndTime,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }
}
