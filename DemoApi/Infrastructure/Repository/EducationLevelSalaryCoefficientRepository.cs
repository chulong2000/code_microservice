using Dapper;
using DemoApi.Domain.IRepository;
using DemoApi.Domain.IServices;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;
using DemoApi.Infrastructure.Data;
using GHM.Infrastructure.Models;
using System.Data;

namespace DemoApi.Infrastructure.Repository
{
    public class EducationLevelSalaryCoefficientRepository : IEducationLevelSalaryCoefficientRepository
    {
        private readonly IDbSession _session;
        public EducationLevelSalaryCoefficientRepository(IDbSession session)
        {
            _session = session;
        }

        public async Task<EducationLevelSalaryCoefficient> GetSalaryCoefficientByEducationLevelId(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);

            var result = await connection.QueryAsync<EducationLevelSalaryCoefficient, EducationLevel, EducationLevelSalaryCoefficient>(
                "[dbo].[spEducationLevelSalaryCoefficient_GetSalaryCoefficientOfEducationLevel]",
                (salary, education) =>
                {
                    salary.educationLevel = education;
                    return salary;
                },
                param,
                transaction: _session.Transaction,
                splitOn: "Id",
                commandType: CommandType.StoredProcedure);

            return result.FirstOrDefault();
        }

        public async Task<int> InsertAsync(EducationLevelSalaryCoefficient entity)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", entity.Id);
            param.Add("@EducationLevelId", entity.EducationLevelId);
            param.Add("@BaseCoefficient", entity.BaseCoefficient);
            param.Add("@AllowancePercentage", entity.AllowancePercentage);
            param.Add("@EffectiveFrom", entity.EffectiveFrom);
            param.Add("@Notes", entity.Notes);
            param.Add("@CreatedAt", entity.CreatedAt);
            

            // Trả về: 1 = thành công, -1 = trùng tên (race condition ở tầng SQL).
            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spEducationLevelSalaryCoefficient_Insert]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<List<EducationLevelSalaryCoefficient>> SelectListAsync()
        {
            var connection = await _session.GetConnectionAsync();

            var result = await connection.QueryAsync<EducationLevelSalaryCoefficient, EducationLevel, EducationLevelSalaryCoefficient>(
                "[dbo].[spEducationLevelSalaryCoefficient_SelectList]",
                (salary, education) =>
                {
                    salary.educationLevel = education;
                    return salary;
                },
                transaction: _session.Transaction,
                splitOn: "Id",
                commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        public async Task<int> SoftDeleteAsync(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spEducationLevelSalaryCoefficient_SoftDelete]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateAsync(EducationLevelSalaryCoefficient entity)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@EducationLevelId", entity.EducationLevelId);
            param.Add("@BaseCoefficient", entity.BaseCoefficient);
            param.Add("@AllowancePercentage", entity.AllowancePercentage);
            param.Add("@EffectiveFrom", entity.EffectiveFrom);
            param.Add("@Notes", entity.Notes);
            param.Add("@CreatedAt", entity.CreatedAt);
            param.Add("@UpdatedAt", entity.UpdatedAt);

            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spEducationLevelSalaryCoefficient_Update]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }
    }
}
