using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Domain.IServices
{
    public interface IWorkScheduleService
    {
        Task<ActionResultResponse<List<WorkScheduleViewModel>>> GetListAsync(Guid? employeeId, Guid? facilityId, DateTime? fromDate, DateTime? toDate, Guid? shiftId);
        Task<ActionResultResponse<List<WorkScheduleViewModel>>> GetUpcomingByEmployeeAsync(Guid employeeId);
        Task<ActionResultResponse<WorkScheduleViewModel>> GetDetailAsync(Guid id);
        Task<ActionResultResponse<Guid>> CreateAsync(WorkScheduleMeta meta);
        Task<ActionResultResponse<WorkScheduleBulkCreateResultViewModel>> BulkCreateMonthlyAsync(WorkScheduleBulkCreateMeta meta);
        Task<ActionResultResponse> UpdateAsync(Guid id, WorkScheduleMeta meta);
        Task<ActionResultResponse> DeleteAsync(Guid id);
        Task<ActionResultResponse<WorkScheduleConflictViewModel>> CheckLeaveConflictAsync(Guid employeeId, DateTime workDate);
        Task<ActionResultResponse> BulkSyncAsync(Guid facilityId, DateTime fromDate, DateTime toDate, WorkScheduleBulkSyncMeta meta);
        Task<ActionResultResponse> PublishBatchAsync(Guid facilityId, DateTime fromDate, DateTime toDate, Guid? publishedBy);
        Task<ActionResultResponse> ConfirmAsync(Guid id, WorkScheduleConfirmMeta meta);
        Task<ActionResultResponse> ConfirmBatchAsync(Guid employeeId, DateTime fromDate, DateTime toDate, WorkScheduleConfirmMeta meta);
        Task<ActionResultResponse> DeclineAsync(Guid id, WorkScheduleRejectMeta meta);
    }
}
