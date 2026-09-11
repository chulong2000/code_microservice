using Dapper;
using DemoApi.Domain.IRepository;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Infrastructure.Data;
using System.Data;

namespace DemoApi.Infrastructure.Repository
{
    public class FacilityRepository : IFacilityRepository
    {
        private readonly IDbSession _session;
        public FacilityRepository(IDbSession session)
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
                "[dbo].[spFacility_ExistsName]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> InsertAsync(Facility entity)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", entity.Id);
            param.Add("@Name", entity.Name);
            param.Add("@Address", entity.Address);
            param.Add("@CreatedAt", entity.CreatedAt);

            // Trả về: 1 = thành công, -1 = trùng tên.
            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spFacility_Insert]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateAsync(Facility entity)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", entity.Id);
            param.Add("@Name", entity.Name);
            param.Add("@Address", entity.Address);
            param.Add("@UpdatedAt", entity.UpdatedAt);

            // 1 = thành công, -1 = trùng tên, 0 = không tìm thấy.
            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spFacility_Update]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> SoftDeleteAsync(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spFacility_SoftDelete]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<(List<Facility> Items, int TotalRecords)> SelectListAsync(PagingRequestMeta request)
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
                "[dbo].[spFacility_SelectList]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);

            var totalRecords = await multi.ReadFirstAsync<int>();
            var items = (await multi.ReadAsync<Facility>()).ToList();

            return (items, totalRecords);
        }

        public async Task<Facility?> SelectByIdAsync(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);

            return await connection.QueryFirstOrDefaultAsync<Facility>(
                "[dbo].[spFacility_SelectById]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> GetCountAllEmployeeOFFacility(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spFacility_Count_Employee]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> GetCountAllShiftOFFacility(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spFacility_Count_Shift]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }
    }
}
