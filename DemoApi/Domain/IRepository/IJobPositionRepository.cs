using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;

namespace DemoApi.Domain.IRepository
{
    public interface IJobPositionRepository
    {
        Task<bool> ExistsNameAsync(string name, Guid? excludeId);
        Task<int> InsertAsync(JobPositionMeta entity);
        Task<int> UpdateAsync(JobPositionMeta entity);
        Task<int> SoftDeleteAsync(Guid id);
        Task<List<JobPositionMeta>> SelectListAsync();
        Task<JobPositionMeta> SelectByIdAsync(Guid id);
    }

}
