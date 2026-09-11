using DemoApi.Domain.Models;

namespace DemoApi.Domain.IRepository
{
    public interface ILeaveRequestRepository
    {
        Task<int> InsertAsync(LeaveRequest entity);
        Task<int> UpdateAsync(LeaveRequest entity);
        Task<int> SoftDeleteAsync(Guid id);
        Task<List<LeaveRequest>> SelectAllAsync();
        Task<LeaveRequest?> SelectByIdAsync(Guid id);
    }
}
