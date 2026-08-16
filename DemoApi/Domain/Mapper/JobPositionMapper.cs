using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;

namespace DemoApi.Domain.Mapper
{
    public class JobPositionMapper
    {
        public static JobPositionViewModel MapToViewModel(JobPosition job) => new()
        {
            Id = job.Id,
            Title = job.Title,
            Department = job.Department,
            IsOpen = job.IsOpen,
            //MinimumEducationLevelId = job.MinimumEducationLevelId,
            //MinimumEducationLevelName = job.MinimumEducationLevel.Name,
            MinimumEducationLevel = job.MinimumEducationLevel,
            OpenSlots = job.OpenSlots
        };

        
    }
}
