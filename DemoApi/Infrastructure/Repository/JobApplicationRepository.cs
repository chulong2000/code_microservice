using Dapper;
using DemoApi.Domain.IRepository;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;
using DemoApi.Infrastructure.Data;
using System.Data;

namespace DemoApi.Infrastructure.Repository
{
    public class JobApplicationRepository : IJobApplicationRepository
    {

        private readonly IDbSession _session;

        public JobApplicationRepository(IDbSession session)
        {
            _session = session;
        }

        public async Task<List<JobApplication>> GetListJobApplicationByJobPositionId(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);

            var result = await connection.QueryAsync<JobApplication>(
                "[dbo].[spJobApplication_GetListJobApplicationByJobPositionId]",
                param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        public async Task<int> InsertAsync(JobApplication entity)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", entity.Id);
            param.Add("@JobPositionId", entity.JobPosition.Id);
            param.Add("@FullName", entity.FullName);
            param.Add("@Email", entity.Email);
            param.Add("@PhoneNumber", entity.PhoneNumber);
            param.Add("@DateOfBirth", entity.DateOfBirth);
            param.Add("@Gender", entity.Gender);
            param.Add("@CvFileUrl", entity.CvFileUrl);
            param.Add("@CoverLetter", entity.CoverLetter);
            param.Add("@YearOfExperience", entity.YearsOfExperience);
            param.Add("@AppliedAt", DateTime.Now);
            param.Add("@CreatedAt", DateTime.Now);


            // Trả về: 1 = thành công, -1 = trùng tên (race condition ở tầng SQL).
            return await connection.ExecuteScalarAsync<int>(
                "[dbo].[spJobApplication_Insert]", param,
                transaction: _session.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<JobApplication> SelectByIdAsync(Guid id)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Id", id);


            var result = await connection.QueryAsync<JobApplication, JobPosition, JobApplication>(
                "[dbo].[spJobApplication_SelectById]",
                (app, job) =>
                {
                    app.JobPosition = job;
                    return app;
                },
                param,
                transaction: _session.Transaction,
                splitOn: "Id, Id",
                commandType: CommandType.StoredProcedure);
            return result.First();
        }

        public async Task<List<JobApplication>> SelectListAsync(JobApplicationSearchMeta search)
        {
            var connection = await _session.GetConnectionAsync();
            var param = new DynamicParameters();
            param.Add("@Keyword", search.Keyword);
            param.Add("@JobPositionId", search.JobPositionId);
            param.Add("@AppliedFrom", search.AppliedFrom);
            param.Add("@AppliedTo", search.AppliedTo);

            var result = await connection.QueryAsync<JobApplication, JobPosition, JobApplication>(
                "[dbo].[spJobApplication_Select]",
                (app, job) =>
                {
                    app.JobPosition = job;
                    return app;
                },
                transaction: _session.Transaction,
                splitOn: "Id, Id",
                commandType: CommandType.StoredProcedure);

            return result.ToList();
        }
    }
}
