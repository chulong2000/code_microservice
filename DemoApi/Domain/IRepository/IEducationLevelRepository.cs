using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;

namespace DemoApi.Domain.IRepository
{
    public interface IEducationLevelRepository
    {
        Task<List<EducationLevelViewModel>> SelectListAsync();
        Task<int> InsertAsync(EducationLevel entity);
        Task<int> UpdateAsync(EducationLevel entity);
        Task<int> SoftDeleteAsync(Guid id, DateTime deletedAt);
        Task<bool> ExistsNameAsync(string name, Guid? excludeId);
    }
}
