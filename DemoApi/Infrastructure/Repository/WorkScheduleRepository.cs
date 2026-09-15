using Dapper;
using DemoApi.Domain.IRepository;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Infrastructure.Data;
using System.Data;

namespace DemoApi.Infrastructure.Repository
{
    public class WorkScheduleRepository : IWorkScheduleRepository
    {
        private readonly IDbSession _session;
        public WorkScheduleRepository(IDbSession session)
        {
            _session = session;
        }

        public async Task<int> InsertAsync(WorkSchedule entity)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", entity.Id);
            param.Add("@EmployeeId", entity.EmployeeId);
            param.Add("@ShiftId", entity.ShiftId);
            param.Add("@FacilityId", entity.FacilityId);
            param.Add("@WorkDate", entity.WorkDate);
            param.Add("@Status", entity.Status);
            param.Add("@Note", entity.Note);
            param.Add("@CreatedBy", entity.CreatedBy);
            param.Add("@CreatedAt", entity.CreatedAt);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spWorkSchedule_Insert]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> BulkInsertAsync(List<WorkSchedule> entities)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@workSchedules", BuildWorkScheduleTable(entities));

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[WorkSchedule_bulkInsert]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        private static SqlMapper.ICustomQueryParameter BuildWorkScheduleTable(List<WorkSchedule> entities)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("EmployeeId", typeof(Guid));
            table.Columns.Add("ShiftId", typeof(Guid));
            table.Columns.Add("FacilityId", typeof(Guid));
            table.Columns.Add("WorkDate", typeof(DateTime));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("Note", typeof(string));
            table.Columns.Add("CreatedBy", typeof(Guid));
            table.Columns.Add("CreatedAt", typeof(DateTime));
            table.Columns.Add("UpdatedAt", typeof(DateTime));
            table.Columns.Add("PublishedAt", typeof(DateTime));
            table.Columns.Add("PublishedBy", typeof(Guid));
            table.Columns.Add("confirmedAt", typeof(DateTime));
            table.Columns.Add("confirmedBy", typeof(Guid));
            table.Columns.Add("IsDeleted",typeof(Boolean));

            foreach (var entity in entities)
            {
                table.Rows.Add(
                    entity.Id,
                    entity.EmployeeId,
                    entity.ShiftId,
                    entity.FacilityId,
                    entity.WorkDate,
                    entity.Status,
                    (object?)entity.Note ?? DBNull.Value,
                    (object?)entity.CreatedBy ?? DBNull.Value,
                    entity.CreatedAt,
                    (object?)entity.UpdatedAt ?? DBNull.Value,
                    (object?)entity.PublishedAt ?? DBNull.Value,
                    (object?)entity.PublishedBy ?? DBNull.Value,
                    (object?)entity.confirmedAt ?? DBNull.Value,
                    (object?)entity.confirmedBy ?? DBNull.Value,
                    entity.IsDeleted);
            }

            return table.AsTableValuedParameter("[dbo].[typWorkSchedule]");
        }

        public async Task<int> UpdateAsync(WorkSchedule entity)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", entity.Id);
            param.Add("@EmployeeId", entity.EmployeeId);
            param.Add("@ShiftId", entity.ShiftId);
            param.Add("@FacilityId", entity.FacilityId);
            param.Add("@WorkDate", entity.WorkDate);
            param.Add("@Status", entity.Status);
            param.Add("@Note", entity.Note);
            param.Add("@UpdatedAt", entity.UpdatedAt);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spWorkSchedule_Update]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> SoftDeleteAsync(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spWorkSchedule_SoftDelete]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<List<WorkSchedule>> SelectAllAsync()
        {
            var connection = await _session.GetConnectionAsync();

            var items = await connection.QueryAsync<WorkSchedule>(
                "[dbo].[spWorkSchedule_SelectAll]",
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);

            return items.ToList();
        }

        public async Task<List<WorkSchedule>> SelectByFilterAsync(Guid? employeeId, Guid? facilityId, DateTime? fromDate, DateTime? toDate, Guid? shiftId)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@EmployeeId", employeeId);
            param.Add("@FacilityId", facilityId);
            param.Add("@FromDate", fromDate);
            param.Add("@ToDate", toDate);
            param.Add("@ShiftId", shiftId);

            var items = await connection.QueryAsync<WorkSchedule>(
                "[dbo].[spWorkSchedule_SelectByFilter]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);

            return items.ToList();
        }

        public async Task<WorkSchedule?> SelectByIdAsync(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);

            return await connection.QueryFirstOrDefaultAsync<WorkSchedule>(
                "[dbo].[spWorkSchedule_SelectById]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<List<WorkSchedule>> SelectDraftInScopeAsync(Guid facilityId, DateTime fromDate, DateTime toDate)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@FacilityId", facilityId);
            param.Add("@FromDate", fromDate);
            param.Add("@ToDate", toDate);

            var items = await connection.QueryAsync<WorkSchedule>(
                "[dbo].[spWorkSchedule_SelectDraftInScope]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);

            return items.ToList();
        }

        public async Task<int> BulkSoftDeleteAsync(List<Guid> ids)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Ids", BuildGuidTable(ids));

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spWorkSchedule_BulkSoftDelete]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        private static SqlMapper.ICustomQueryParameter BuildGuidTable(List<Guid> ids)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(Guid));

            foreach (var id in ids)
            {
                table.Rows.Add(id);
            }

            return table.AsTableValuedParameter("[dbo].[GuidListType]");
        }

        public async Task<int> PublishBatchAsync(Guid facilityId, DateTime fromDate, DateTime toDate, Guid? publishedBy, DateTime publishedAt)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@FacilityId", facilityId);
            param.Add("@FromDate", fromDate);
            param.Add("@ToDate", toDate);
            param.Add("@PublishedBy", publishedBy);
            param.Add("@PublishedAt", publishedAt);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spWorkSchedule_PublishBatch]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> ConfirmAsync(Guid id, Guid confirmedBy, DateTime confirmedAt)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);
            param.Add("@ConfirmedBy", confirmedBy);
            param.Add("@ConfirmedAt", confirmedAt);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spWorkSchedule_Confirm]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> ConfirmBatchAsync(Guid employeeId, DateTime fromDate, DateTime toDate, DateTime confirmedAt)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@EmployeeId", employeeId);
            param.Add("@FromDate", fromDate);
            param.Add("@ToDate", toDate);
            param.Add("@ConfirmedAt", confirmedAt);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spWorkSchedule_ConfirmBatch]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeclineAsync(Guid id, WorkScheduleRejectMeta meta, DateTime now)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);
            param.Add("@CancelledBy", meta.RejectedBy);
            param.Add("@Note", meta.Note);
            param.Add("@CancelledAt", now);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spWorkSchedule_Decline]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }
    }
}
