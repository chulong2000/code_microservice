using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;

namespace DemoApi.Domain.IRepository
{
    public interface IEducationLevelRepository
    {
        Task<bool> ExistsNameAsync(string name, Guid? excludeId);
        Task<int> InsertAsync(EducationLevel entity);
        Task<int> UpdateAsync(EducationLevel entity);
        Task<int> SoftDeleteAsync(Guid id);
        Task<(List<EducationLevel> Items, int TotalRecords)> SelectListAsync(PagingRequestMeta request);
        Task<EducationLevel?> SelectByIdAsync(Guid id);
        Task<List<JobPosition>> GetListJobPositionByEducationLevelId(Guid id);
    }
}
