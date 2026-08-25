using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Domain.IServices
{
    public interface IJobApplicationService
    {
        Task<ActionResultResponse<Guid>> CreateAsync(JobApplicationMeta meta);
        Task<ActionResultResponse> DeleteAsync(Guid id);
        Task<ActionResultResponse<JobApplicationViewModel>> GetDetailAsync(Guid id);
        Task<ActionResultResponse<List<JobApplicationViewModel>>> GetListAsync(JobApplicationSearchMeta search);
        Task<ActionResultResponse<List<JobApplicationViewModel>>> GetListJobApplicationByJobApplicationId(Guid id);
        Task<ActionResultResponse> UpdateAsync(Guid id, JobApplicationMeta meta);
    }
}
