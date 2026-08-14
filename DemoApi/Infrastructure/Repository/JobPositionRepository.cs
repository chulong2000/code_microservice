using Dapper;
using DemoApi.Domain.IRepository;
using DemoApi.Domain.ModelMetas;
using DemoApi.Infrastructure.Data;
using System.Data;

namespace DemoApi.Infrastructure.Repository
{
    public class JobPositionRepository : IJobPositionRepository
    {
        private readonly IDbSession _session;

        public JobPositionRepository (IDbSession session)
        {
            _session = session;
        }
        public async  Task<bool> ExistsNameAsync(string name, Guid? educationLevelId)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Name", name);
            param.Add("@ExcludeId", educationLevelId);

            return await connection.ExecuteScalarAsync<bool>(
                "[dbo].[spEducationLevel_ExistsName]", param,
                transaction: _session.Transaction,          // luôn truyền, kể cả read
                commandType: CommandType.StoredProcedure);
        }

        public Task<int> InsertAsync(JobPositionMeta entity)
        {
            throw new NotImplementedException();
        }

        public Task<JobPositionMeta> SelectByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<JobPositionMeta>> SelectListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> SoftDeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(JobPositionMeta entity)
        {
            throw new NotImplementedException();
        }
    }
}
