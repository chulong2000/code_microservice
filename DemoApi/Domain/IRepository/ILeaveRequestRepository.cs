using DemoApi.Domain.Models;

namespace DemoApi.Domain.IRepository
{
    public interface ILeaveRequestRepository
    {
        Task<int> InsertAsync(LeaveRequest entity);
        Task<int> UpdateAsync(LeaveRequest entity);
        Task<int> SoftDeleteAsync(Guid id);
        Task<List<LeaveRequest>> SelectAllAsync();
        Task<List<LeaveRequest>> SelectByFilterAsync(Guid? employeeId, string? status);
        Task<LeaveRequest?> SelectByIdAsync(Guid id);
        Task<int> ApproveAsync(Guid id, Guid? approvedBy, DateTime approvedAt);
        Task<int> RejectAsync(Guid id, Guid? approvedBy, DateTime approvedAt);
    }
}
