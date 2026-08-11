using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Domain.IServices
{
    public interface IEducationLevelService
    {
        Task<ActionResultResponse<List<EducationLevelViewModel>>> GetListAsync();
        Task<ActionResultResponse<EducationLevelViewModel>> GetDetailAsync(Guid id);
        Task<ActionResultResponse<Guid>> CreateAsync(EducationLevelMeta meta);
        Task<ActionResultResponse> UpdateAsync(Guid id, EducationLevelMeta meta);
        Task<ActionResultResponse> DeleteAsync(Guid id);
    }
}
