using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;

namespace DemoApi.Domain.Mapper
{
    public class WorkScheduleMapper
    {
        public static WorkScheduleViewModel MapToViewModel(WorkSchedule entity) => new()
        {
            Id = entity.Id,
            EmployeeId = entity.EmployeeId,
            ShiftId = entity.ShiftId,
            FacilityId = entity.FacilityId,
            WorkDate = entity.WorkDate,
            Status = entity.Status,
            Note = entity.Note,
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }
}
