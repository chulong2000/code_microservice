using DemoApi.Domain.Exceptions;
using DemoApi.Domain.IRepository;
using DemoApi.Domain.IServices;
using DemoApi.Domain.Mapper;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Infrastructure.Service
{
    public class JobApplicationService(IJobApplicationRepository _jobApplicationRepo) : IJobApplicationService
    {
        public async Task<ActionResultResponse<Guid>> CreateAsync(JobApplicationMeta meta)
        {

            var entity = new JobApplication
            {
                Id = Guid.NewGuid(),
                FullName = meta.FullName,
                PhoneNumber = meta.PhoneNumber,
                DateOfBirth = meta.DateOfBirth,
                Email = meta.Email,
                Gender = meta.Gender,
                CvFileUrl = meta.CvFileUrl,
                CoverLetter = meta.CoverLetter,
                YearsOfExperience = meta.YearsOfExperience,
                AppliedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                JobPositionId = meta.JobPositionId
            };

            Console.WriteLine($"Kiểm tra 2345:{entity.JobPosition}");
            return new ActionResultResponse<Guid>(1, "Tạo trình độ học vấn thành công.", data: entity.Id);
        }

        public async Task<ActionResultResponse> DeleteAsync(Guid id)
        {
            var result = await _jobApplicationRepo.SoftDeleteAsync(id);

            return result <= 0 
                ? new ActionResultResponse(-99, "Không tìm thấy CV này.")
                : new ActionResultResponse(1, "Xóa thành công");
        }

        public async Task<ActionResultResponse<JobApplicationViewModel>> GetDetailAsync(Guid id)
        {
            var entity = await _jobApplicationRepo.SelectByIdAsync(id);
            return entity is null 
                ? new ActionResultResponse<JobApplicationViewModel>(-99, "Không tìm thấy hồ sơ ứng tuyển")
                : new ActionResultResponse<JobApplicationViewModel>(JobApplicationMapper.MapToViewModel(entity));
        }

        public async Task<ActionResultResponse<List<JobApplicationViewModel>>> GetListAsync(JobApplicationSearchMeta search)
        {
            var entities = await _jobApplicationRepo.SelectListAsync(search);
            var data = entities.Select(x => JobApplicationMapper.MapToViewModel(x)).ToList();

            // Constructor (T data) tự set Code = 1 — dùng cho case thành công đơn giản, không cần message riêng.
            return new ActionResultResponse<List<JobApplicationViewModel>>(data);
        }

        public async Task<ActionResultResponse<List<JobApplicationViewModel>>> GetListJobApplicationByJobApplicationId(Guid id)
        {
            var entities = await _jobApplicationRepo.GetListJobApplicationByJobPositionId(id);
            var data = entities.Select(x => JobApplicationMapper.MapToViewModel(x)).ToList();

            return new ActionResultResponse<List<JobApplicationViewModel>>(data);
        }

        public async Task<ActionResultResponse> UpdateAsync(Guid id, JobApplicationMeta meta)
        {
            var entity = new JobApplication
            {
                Id = id,
                JobPositionId = meta.JobPositionId,
                FullName = meta.FullName,
                Email = meta.Email,
                PhoneNumber = meta.PhoneNumber,
                YearsOfExperience = meta.YearsOfExperience,
                CvFileUrl = meta.CvFileUrl,
                AppliedAt = meta.AppliedAt,
                UpdatedAt = DateTime.Now,
                CoverLetter = meta.CoverLetter,
                DateOfBirth = meta.DateOfBirth,
                Gender = meta.Gender 
            };

            var result = await _jobApplicationRepo.UpdateAsync(entity);

            return result <= 0 
                ? new ActionResultResponse(-90, "Không tìm thấy thông tin về hồ sơ ứng tuyển này.")
                : new ActionResultResponse(1, "Cập nhập thành công.");
        }
    }
}
