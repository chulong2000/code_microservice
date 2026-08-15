using DemoApi.Domain.IRepository;
using DemoApi.Domain.IServices;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Infrastructure.Service
{
    public class JobPositionService : IJobPositionService
    {
        private readonly IJobPositionRepository _jobPositionRepo;

        // .NET cần "tiêm" IJobPositionService vào đây
        public JobPositionService(IJobPositionRepository jobPositionRepo)
        {
            _jobPositionRepo = jobPositionRepo;
        }
        public async Task<ActionResultResponse<Guid>> CreateAsync(JobPositionMeta meta)
        {
            var name = meta.Title.Trim();

            if (await _jobPositionRepo.ExistsNameAsync(name, null))
                return new ActionResultResponse<Guid>(-1, $"Vị trí công việc \"{name}\" đã tồn tại.");

            var entity = new JobPosition
            {
                Id = Guid.NewGuid(),
                Title = name,
                Department = meta.Department,
                OpenSlots = meta.OpenSlots,
                MinimumEducationLevelId = meta.MinimumEducationLevelId,
                IsOpen = meta.IsOpen,
                CreatedAt = meta.CreatedAt
            };

            var result = await _jobPositionRepo.InsertAsync(entity);
            if (result <= 0)
            {
                return result == -1
                    ? new ActionResultResponse<Guid>(-1, $"Vị trí \"{name}\" đã tồn tại.")
                    : new ActionResultResponse<Guid>(-99, "Có lỗi xảy ra, vui lòng thử lại.");
            }

            // Tham số thứ 3 của constructor thật là "title", không phải "data" -> phải truyền data bằng named argument.
            return new ActionResultResponse<Guid>(1, "Tạo vị trí công việc mới thành công.", data: entity.Id);
        }

        public async Task<ActionResultResponse> DeleteAsync(Guid id)
        {
            var result = await _jobPositionRepo.SoftDeleteAsync(id);
            return result <= 0
                ? new ActionResultResponse(-99, "Không tìm thấy vị trí công việc.")
                : new ActionResultResponse(1, "Xoá thành công.");
        }

        public async Task<ActionResultResponse<JobPositionViewModel>> GetDetailAsync(Guid id)
        {
            var entity = await _jobPositionRepo.SelectByIdAsync(id);
            if (entity is null)
                return new ActionResultResponse<JobPositionViewModel>(-99, "Không tìm thấy trình độ học vấn.");

            return new ActionResultResponse<JobPositionViewModel>(MapToViewModel(entity));
        }

        public async Task<ActionResultResponse<List<JobPositionViewModel>>> GetListAsync(Guid educationLevelId, string keyword)
        {
            var entities = await _jobPositionRepo.SelectListAsync(educationLevelId, keyword);
            var data = entities.Select(MapToViewModel).ToList();

            // Constructor (T data) tự set Code = 1 — dùng cho case thành công đơn giản, không cần message riêng.
            return new ActionResultResponse<List<JobPositionViewModel>>(data);
        }

        public async Task<ActionResultResponse> UpdateAsync(Guid id, JobPositionMeta meta)
        {
            var name = meta.Title.Trim();

            if (await _jobPositionRepo.ExistsNameAsync(name, id))
                return new ActionResultResponse(-1, $"Trình độ học vấn \"{name}\" đã tồn tại.");

            var entity = new JobPosition
            {
                Id = id,
                Title = name,
                OpenSlots = meta.OpenSlots,
                MinimumEducationLevelId = meta.MinimumEducationLevelId,
                IsOpen = meta.IsOpen
            };

            var result = await _jobPositionRepo.UpdateAsync(entity);
            return result switch
            {
                1 => new ActionResultResponse(1, "Cập nhật thành công."),
                -1 => new ActionResultResponse(-1, $"Vị trí công việc \"{name}\" đã tồn tại."),
                _ => new ActionResultResponse(-99, "Không tìm thấy vị trí công việc.")
            };
        }

        private static JobPositionViewModel MapToViewModel(JobPosition job) => new()
        {
            Id = job.Id,
            Title = job.Title,
            Department = job.Department,
            IsOpen = job.IsOpen,
            MinimumEducationLevelId = job.MinimumEducationLevel.Id,
            MinimumEducationLevelName = job.MinimumEducationLevel.Name,
            OpenSlots = job.OpenSlots
        };

        
    }
}
