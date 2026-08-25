using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Domain.IServices
{
    public interface IEducationLevelSalaryCoefficientService
    {
        Task<ActionResultResponse> DeleteAsync(Guid id);
        Task<ActionResultResponse<List<EducationLevelSalaryCoefficientViewModel>>> GetListAsync();
        Task<ActionResultResponse<EducationLevelSalaryCoefficientViewModel>> GetSalaryCoefficientByEducationLevelId(Guid id);
        Task<ActionResultResponse> UpdateAsync(EducationLevelSalaryCoefficientMeta meta);

        Task<ActionResultResponse<Guid>> CreateAsync(EducationLevelSalaryCoefficientMeta meta);
    }
}
