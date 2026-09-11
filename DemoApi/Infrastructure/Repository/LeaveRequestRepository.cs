using Dapper;
using DemoApi.Domain.IRepository;
using DemoApi.Domain.Models;
using DemoApi.Infrastructure.Data;
using System.Data;

namespace DemoApi.Infrastructure.Repository
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly IDbSession _session;
        public LeaveRequestRepository(IDbSession session)
        {
            _session = session;
        }

        public async Task<int> InsertAsync(LeaveRequest entity)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", entity.Id);
            param.Add("@EmployeeId", entity.EmployeeId);
            param.Add("@LeaveType", entity.LeaveType);
            param.Add("@FromDate", entity.FromDate);
            param.Add("@ToDate", entity.ToDate);
            param.Add("@Reason", entity.Reason);
            param.Add("@Status", entity.Status);
            param.Add("@ApprovedBy", entity.ApprovedBy);
            param.Add("@ApprovedAt", entity.ApprovedAt);
            param.Add("@CreatedAt", entity.CreatedAt);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spLeaveRequest_Insert]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateAsync(LeaveRequest entity)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", entity.Id);
            param.Add("@EmployeeId", entity.EmployeeId);
            param.Add("@LeaveType", entity.LeaveType);
            param.Add("@FromDate", entity.FromDate);
            param.Add("@ToDate", entity.ToDate);
            param.Add("@Reason", entity.Reason);
            param.Add("@Status", entity.Status);
            param.Add("@ApprovedBy", entity.ApprovedBy);
            param.Add("@ApprovedAt", entity.ApprovedAt);
            param.Add("@UpdatedAt", entity.UpdatedAt);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spLeaveRequest_Update]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> SoftDeleteAsync(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spLeaveRequest_SoftDelete]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<List<LeaveRequest>> SelectAllAsync()
        {
            var connection = await _session.GetConnectionAsync();

            var items = await connection.QueryAsync<LeaveRequest>(
                "[dbo].[spLeaveRequest_SelectAll]",
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);

            return items.ToList();
        }

        public async Task<LeaveRequest?> SelectByIdAsync(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);

            return await connection.QueryFirstOrDefaultAsync<LeaveRequest>(
                "[dbo].[spLeaveRequest_SelectById]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }
    }
}
