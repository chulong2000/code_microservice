using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;

namespace DemoApi.Domain.Mapper
{
    public class LeaveRequestMapper
    {
        public static LeaveRequestViewModel MapToViewModel(LeaveRequest entity) => new()
        {
            Id = entity.Id,
            EmployeeId = entity.EmployeeId,
            LeaveType = entity.LeaveType,
            FromDate = entity.FromDate,
            ToDate = entity.ToDate,
            Reason = entity.Reason,
            Status = entity.Status,
            ApprovedBy = entity.ApprovedBy,
            ApprovedAt = entity.ApprovedAt,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }
}
