using Dapper;
using DemoApi.Domain.IRepository;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
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
        public async  Task<bool> ExistsNameAsync(string title, Guid? educationLevelId)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Title", title);
            param.Add("@ExcludeId", educationLevelId);

            return await connection.ExecuteScalarAsync<bool>(
                "[dbo].[spJobPosition_ExistsName]", param,
                transaction: _session.Transaction,          // luôn truyền, kể cả read
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> InsertAsync(JobPosition entity)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", entity.Id);
            param.Add("@Title", entity.Title);
            param.Add("@Department", entity.Department);
            param.Add("@EducationLevelId",entity.MinimumEducationLevelId);
            param.Add("@OpenSlots", entity.OpenSlots);
            param.Add("@IsOpen", entity.IsOpen);
            param.Add("@CreatedAt", entity.CreatedAt);
            param.Add("@IsDeleted", false);

            // Trả về: 1 = thành công, -1 = trùng tên (race condition ở tầng SQL).
            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spJobPosition_Insert]", param,
                
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<JobPosition> SelectByIdAsync(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);
          

            return await connection.QueryFirstOrDefaultAsync<JobPosition>(
                "[dbo].[spJobPosition_SelectById]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<List<JobPosition>> SelectListAsync(Guid? educationLevelId, string keyword)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Keyword", keyword);
            param.Add("@EducationId", educationLevelId);

            var result = await connection.QueryAsync<JobPosition, EducationLevel, JobPosition>(
                "[dbo].[spJobPosition_SelectList]",
                (job, education) =>
                {
                    job.MinimumEducationLevel = education;
                    return job;
                },
                param,
                transaction: _session.Transaction,
                splitOn: "Id",
                commandType: CommandType.StoredProcedure);

            Console.WriteLine("Check_23455: " + result);
            return result.ToList();
        }

        public async Task<int> SoftDeleteAsync(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spJobPosition_SoftDelete]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateAsync(JobPosition entity)
        {
            Console.WriteLine("Check_567: "+ entity);
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", entity.Id);
            param.Add("@Title", entity.Title);
            param.Add("@Department", entity.Department);
            param.Add("@OpenSlots", entity.OpenSlots);
            param.Add("@EducationLevelId", entity.MinimumEducationLevelId);
            param.Add("@IsOpen", entity.IsOpen);
            param.Add("@UpdatedAt", entity.UpdatedAt);

            // 1 = thành công, -1 = trùng tên, 0 = không tìm thấy.
            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spJobPosition_Update]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }
    }
}
