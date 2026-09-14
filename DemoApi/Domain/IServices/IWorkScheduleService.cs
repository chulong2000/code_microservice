using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Domain.IServices
{
    public interface IWorkScheduleService
    {
        Task<ActionResultResponse<List<WorkScheduleViewModel>>> GetListAsync();
        Task<ActionResultResponse<WorkScheduleViewModel>> GetDetailAsync(Guid id);
        Task<ActionResultResponse<Guid>> CreateAsync(WorkScheduleMeta meta);
        Task<ActionResultResponse> BulkCreateMonthlyAsync(WorkScheduleBulkCreateMeta meta);
        Task<ActionResultResponse> UpdateAsync(Guid id, WorkScheduleMeta meta);
        Task<ActionResultResponse> DeleteAsync(Guid id);
    }
}
