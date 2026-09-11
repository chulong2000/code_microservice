using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;

namespace DemoApi.Domain.Mapper
{
    public class FacilityMapper
    {
        public static FacilityViewModel MapToViewModel(Facility entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Address = entity.Address,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }
}
