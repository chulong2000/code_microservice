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
                Gender = meta.Gender,
                CvFileUrl = meta.CvFileUrl,
                CoverLetter = meta.CoverLetter,
                YearsOfExperience = meta.YearsOfExperience,
                AppliedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                JobPositionId = meta.JobPositionId
            };

            Console.WriteLine($"Kiểm tra 2345:{entity.JobPosition}");

            var result = await _jobApplicationRepo.InsertAsync(entity);
            if (result <= 0)
            {
                new ActionResultResponse<Guid>(-99, "Ứng tuyển CV thất bại.");
            }
            // Tham số thứ 3 của constructor thật là "title", không phải "data" -> phải truyền data bằng named argument.
            return new ActionResultResponse<Guid>(1, "Tạo trình độ học vấn thành công.", data: entity.Id);
        }

        public async Task<ActionResultResponse<JobApplicationViewModel>> GetDetailAsync(Guid id)
        {
            var entity = await _jobApplicationRepo.SelectByIdAsync(id);
            if (entity is null)
                return new ActionResultResponse<JobApplicationViewModel>(-99, "Không tìm thấy trình độ học vấn.");

            return new ActionResultResponse<JobApplicationViewModel>(JobApplicationMapper.MapToViewModel(entity));
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
    }
}
