using Dapper;
using DemoApi.Domain.IRepository;
using DemoApi.Domain.Models;
using DemoApi.Infrastructure.Data;
using System.Data;

namespace DemoApi.Infrastructure.Repository
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly IDbSession _session;
        public ShiftRepository(IDbSession session)
        {
            _session = session;
        }

        public async Task<int> InsertAsync(Shift entity)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", entity.Id);
            param.Add("@FacilityId", entity.FacilityId);
            param.Add("@Name", entity.Name);
            param.Add("@StartTime", entity.StartTime);
            param.Add("@EndTime", entity.EndTime);
            param.Add("@CreatedAt", entity.CreatedAt);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spShift_Insert]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateAsync(Shift entity)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", entity.Id);
            param.Add("@FacilityId", entity.FacilityId);
            param.Add("@Name", entity.Name);
            param.Add("@StartTime", entity.StartTime);
            param.Add("@EndTime", entity.EndTime);
            param.Add("@UpdatedAt", entity.UpdatedAt);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spShift_Update]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> SoftDeleteAsync(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spShift_SoftDelete]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<List<Shift>> SelectAllAsync()
        {
            var connection = await _session.GetConnectionAsync();

            var items = await connection.QueryAsync<Shift>(
                "[dbo].[spShift_SelectAll]",
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);

            return items.ToList();
        }

        public async Task<List<Shift>> SelectByIdAsync(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);

            var items = await connection.QueryAsync<Shift, Facility, Shift>(
                "[dbo].[spShift_SelectById]",
                (shift, facility) =>
                {
                    shift.Facility = facility;
                    return shift;
                },
                param,
                transaction: _session.Transaction,
                splitOn: "Id",
                commandType: CommandType.StoredProcedure);

            return items.ToList();
        }
    }
}
