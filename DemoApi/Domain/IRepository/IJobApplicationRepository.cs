using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;

namespace DemoApi.Domain.IRepository
{
    public interface IJobApplicationRepository
    {
        Task<List<JobApplication>> GetListJobApplicationByJobPositionId(Guid id);
        Task<int> InsertAsync(JobApplication entity);
        Task<JobApplication> SelectByIdAsync(Guid id);
        Task<List<JobApplication>> SelectListAsync(JobApplicationSearchMeta search);
        Task<int> SoftDeleteAsync(Guid id);
        Task<int> UpdateAsync(JobApplication entity);
    }
}
