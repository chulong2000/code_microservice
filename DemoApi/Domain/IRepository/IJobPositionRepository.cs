using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;

namespace DemoApi.Domain.IRepository
{
    public interface IJobPositionRepository
    {
        Task<bool> ExistsNameAsync(string name, Guid? excludeId);
        Task<int> InsertAsync(JobPosition entity);
        Task<int> UpdateAsync(JobPosition entity);
        Task<int> SoftDeleteAsync(Guid id);
        Task<List<JobPosition>> SelectListAsync(Guid educationLevelId, string keyword);
        Task<JobPosition> SelectByIdAsync(Guid id);
    }

}
