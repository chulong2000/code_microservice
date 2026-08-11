using Dapper;
using DemoApi.Domain.IRepository;
using DemoApi.Domain.Models;
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

        public async Task<List<EducationLevel>> SelectListAsync()
        {
            var connection = await _session.GetConnectionAsync();

            var result = await connection.QueryAsync<EducationLevel>(
                "[dbo].[spEducationLevel_SelectList]",
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        public async Task<EducationLevel?> SelectByIdAsync(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);

            return await connection.QueryFirstOrDefaultAsync<EducationLevel>(
                "[dbo].[spEducationLevel_SelectById]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }




    }
}
