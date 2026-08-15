using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Domain.IServices
{
    public interface IJobPositionService
    {
        Task<ActionResultResponse<Guid>> CreateAsync(JobPositionMeta meta);
        Task<ActionResultResponse> DeleteAsync(Guid id);
        Task<ActionResultResponse<JobPositionViewModel>> GetDetailAsync(Guid id);
        Task<ActionResultResponse<List<JobPositionViewModel>>> GetListAsync(Guid educationLevelId, string keyword);
        Task<ActionResultResponse> UpdateAsync(Guid id, JobPositionMeta meta);
    }
}
