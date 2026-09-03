using Dapper;
using DemoApi.Domain.IRepository;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;
using DemoApi.Infrastructure.Data;
using System.Data;

namespace DemoApi.Infrastructure.Repository
{
    public class EducationLevelRepository : IEducationLevelRepository
    {
        private readonly IDbSession _session;
        public EducationLevelRepository(IDbSession session)
        {
            _session = session;
        }

        public async Task<bool> ExistsNameAsync(string name, Guid? excludeId)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Name", name);
            param.Add("@ExcludeId", excludeId);

            return await connection.ExecuteScalarAsync<bool>(
                "[dbo].[spEducationLevel_ExistsName]", param,
                transaction: _session.Transaction,          // luôn truyền, kể cả read
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> InsertAsync(EducationLevel entity)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", entity.Id);
            param.Add("@Name", entity.Name);
            param.Add("@Description", entity.Description);
            param.Add("@Order", entity.Order);
            param.Add("@CreatedAt", entity.CreatedAt);

            // Trả về: 1 = thành công, -1 = trùng tên (race condition ở tầng SQL).
            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spEducationLevel_Insert]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }
        
        public async Task<int> UpdateAsync(EducationLevel entity)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", entity.Id);
            param.Add("@Name", entity.Name);
            param.Add("@Description", entity.Description);
            param.Add("@Order", entity.Order);
            param.Add("@UpdatedAt", entity.UpdatedAt);

            // 1 = thành công, -1 = trùng tên, 0 = không tìm thấy.
            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spEducationLevel_Update]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> SoftDeleteAsync(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spEducationLevel_SoftDelete]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<(List<EducationLevel> Items, int TotalRecords)> SelectListAsync(PagingRequestMeta request)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Keyword", string.IsNullOrWhiteSpace(request.Keyword) ? null : request.Keyword.Trim());
            param.Add("@SortColumn", request.SortColumn);
            param.Add("@SortDescending", request.SortDescending);
            param.Add("@PageIndex", request.PageIndex);
            param.Add("@PageSize", request.PageSize);

            // Stored procedure trả về 2 result set: (1) tổng số bản ghi, (2) dữ liệu của trang hiện tại.
            using var multi = await connection.QueryMultipleAsync(
                "[dbo].[spEducationLevel_SelectList]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);

            var totalRecords = await multi.ReadFirstAsync<int>();
            var items = (await multi.ReadAsync<EducationLevel>()).ToList();

            return (items, totalRecords);
        }

        public async Task<EducationLevel?> SelectByIdAsync(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);

            EducationLevel? educationLevel = null;

            var result = await connection.QueryAsync<EducationLevel, EducationLevelSalaryCoefficient, JobPosition, JobApplication, EducationLevel>(
                "[dbo].[spEducationLevel_SelectById]",
                (edu, sa, job, app) =>
                {
                    educationLevel ??= edu;
                    educationLevel.educationLevelSalaryCoefficient = sa;
                    
                    if (educationLevel.jobPositions.All(j => j.Id != job.Id))
                    {
                        
                        job.jobApplications.Add(app);
                        educationLevel.jobPositions.Add(job);
                    } else
                    {
                        educationLevel.jobPositions.Single(j => j.Id == job.Id).jobApplications.Add(app);
                    }
                    
                    return educationLevel;
                },
                param,
                transaction: _session.Transaction,
                splitOn: "Id, Id, Id",
                commandType: CommandType.StoredProcedure);

            return educationLevel;
        }

        public async Task<List<JobPosition>> GetListJobPositionByEducationLevelId(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);

            var result = await connection.QueryAsync<JobPosition>(
                "[dbo].[spJobPosition_SelectListJobPostionByEducationLevelId]",
                param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);

            return result.ToList();

        }
    }
}
