using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;

namespace DemoApi.Domain.Mapper
{
    public class EducationLevelMapper
    {
        
        public static EducationLevelViewModel MapToViewModel(EducationLevel entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Order = entity.Order,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };


    }
}
