using DemoApi.Domain.ModelMetas;
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
        Task<List<WorkSchedule>> SelectByFilterAsync(Guid? employeeId, Guid? facilityId, DateTime? fromDate, DateTime? toDate, Guid? shiftId);
        Task<WorkSchedule?> SelectByIdAsync(Guid id);
        Task<List<WorkSchedule>> SelectDraftInScopeAsync(Guid facilityId, DateTime fromDate, DateTime toDate);
        Task<int> BulkSoftDeleteAsync(List<Guid> ids);
        Task<int> PublishBatchAsync(Guid facilityId, DateTime fromDate, DateTime toDate, Guid? publishedBy, DateTime publishedAt);
        Task<int> ConfirmAsync(Guid id, Guid confirmedBy, DateTime confirmedAt);
        Task<int> ConfirmBatchAsync(Guid employeeId, DateTime fromDate, DateTime toDate, DateTime confirmedAt);
        Task<int> DeclineAsync(Guid id, WorkScheduleRejectMeta meta, DateTime now);
    }
}
