using DemoApi.Domain.IRepository;
using DemoApi.Domain.IServices;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Infrastructure.Service
{
    public class JobPositionService : IJobPositionService
    {
        private readonly IJobPositionRepository _jobPositionRepo;
        public Task<ActionResultResponse<Guid>> CreateAsync(JobPositionMeta meta)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResultResponse> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResultResponse<JobPositionViewModel>> GetDetailAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResultResponse<List<JobPositionViewModel>>> GetListAsync(Guid educationLevelId, string keyword)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResultResponse> UpdateAsync(Guid id, JobPosition meta)
        {
            throw new NotImplementedException();
        }
    }
}
