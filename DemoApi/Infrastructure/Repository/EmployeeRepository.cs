using Dapper;
using DemoApi.Domain.IRepository;
using DemoApi.Domain.Models;
using DemoApi.Infrastructure.Data;
using System.Data;

namespace DemoApi.Infrastructure.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbSession _session;
        public EmployeeRepository(IDbSession session)
        {
            _session = session;
        }

        public async Task<int> InsertAsync(Employee entity)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", entity.Id);
            param.Add("@EmployeeCode", entity.EmployeeCode);
            param.Add("@JobApplicationId", entity.JobApplicationId);
            param.Add("@JobPositionId", entity.JobPositionId);
            param.Add("@PrimaryFacilityId", entity.PrimaryFacilityId);
            param.Add("@FullName", entity.FullName);
            param.Add("@Email", entity.Email);
            param.Add("@PhoneNumber", entity.PhoneNumber);
            param.Add("@DateOfBirth", entity.DateOfBirth);
            param.Add("@Gender", entity.Gender);
            param.Add("@HireDate", entity.HireDate);
            param.Add("@Status", entity.Status);
            param.Add("@CreatedAt", entity.CreatedAt);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spEmployee_Insert]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateAsync(Employee entity)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", entity.Id);
            param.Add("@EmployeeCode", entity.EmployeeCode);
            param.Add("@JobApplicationId", entity.JobApplicationId);
            param.Add("@JobPositionId", entity.JobPositionId);
            param.Add("@PrimaryFacilityId", entity.PrimaryFacilityId);
            param.Add("@FullName", entity.FullName);
            param.Add("@Email", entity.Email);
            param.Add("@PhoneNumber", entity.PhoneNumber);
            param.Add("@DateOfBirth", entity.DateOfBirth);
            param.Add("@Gender", entity.Gender);
            param.Add("@HireDate", entity.HireDate);
            param.Add("@Status", entity.Status);
            param.Add("@UpdatedAt", entity.UpdatedAt);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spEmployee_Update]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> SoftDeleteAsync(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spEmployee_SoftDelete]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<List<Employee>> SelectAllAsync()
        {
            var connection = await _session.GetConnectionAsync();

            var items = await connection.QueryAsync<Employee>(
                "[dbo].[spEmployee_SelectAll]",
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);

            return items.ToList();
        }

        public async Task<Employee?> SelectByIdAsync(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);

            return await connection.QueryFirstOrDefaultAsync<Employee>(
                "[dbo].[spEmployee_SelectById]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }
    }
}
