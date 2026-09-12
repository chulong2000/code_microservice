using DemoApi.Domain.Models;

namespace DemoApi.Domain.IRepository
{
    public interface IWorkScheduleRepository
    {
        Task<int> InsertAsync(WorkSchedule entity);
        Task<int> BulkInsertAsync(List<WorkSchedule> entities);
        Task<int> UpdateAsync(WorkSchedule entity);
        Task<int> SoftDeleteAsync(Guid id);
        Task<List<WorkSchedule>> SelectAllAsync();
        Task<WorkSchedule?> SelectByIdAsync(Guid id);
    }
}
