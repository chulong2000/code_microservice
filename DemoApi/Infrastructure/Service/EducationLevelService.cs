using DemoApi.Domain.IRepository;
using DemoApi.Domain.IServices;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Infrastructure.Service
{
    public class EducationLevelService : IEducationLevelService
    {
        private readonly IEducationLevelRepository _educationRepo;

        public EducationLevelService (IEducationLevelRepository educationRepo)
        {
            _educationRepo = educationRepo;
        }

        public async Task<ActionResultResponse<List<EducationLevelViewModel>>> GetListAsync()
        {
            var entities = await _educationRepo.SelectListAsync();
            var data = entities.Select(MapToViewModel).ToList();

            // Constructor (T data) tự set Code = 1 — dùng cho case thành công đơn giản, không cần message riêng.
            return new ActionResultResponse<List<EducationLevelViewModel>>(data);
        }

        public async Task<ActionResultResponse<EducationLevelViewModel>> GetDetailAsync(Guid id)
        {
            var entity = await _educationRepo.SelectByIdAsync(id);
            if (entity is null)
                return new ActionResultResponse<EducationLevelViewModel>(-99, "Không tìm thấy trình độ học vấn.");

            return new ActionResultResponse<EducationLevelViewModel>(MapToViewModel(entity));
        }

        public async Task<ActionResultResponse<Guid>> CreateAsync(EducationLevelMeta meta)
        {
            var name = meta.Name.Trim();

            if (await _educationRepo.ExistsNameAsync(name, null))
                return new ActionResultResponse<Guid>(-1, $"Trình độ học vấn \"{name}\" đã tồn tại.");

            var entity = new EducationLevel
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = meta.Description?.Trim(),
                Order = meta.Order,
                CreatedAt = DateTime.Now
            };

            var result = await _educationRepo.InsertAsync(entity);
            if (result <= 0)
            {
                return result == -1
                    ? new ActionResultResponse<Guid>(-1, $"Trình độ học vấn \"{name}\" đã tồn tại.")
                    : new ActionResultResponse<Guid>(-99, "Có lỗi xảy ra, vui lòng thử lại.");
            }

            // Tham số thứ 3 của constructor thật là "title", không phải "data" -> phải truyền data bằng named argument.
            return new ActionResultResponse<Guid>(1, "Tạo trình độ học vấn thành công.", data: entity.Id);
        }

        public async Task<ActionResultResponse> UpdateAsync(Guid id, EducationLevelMeta meta)
        {
            var name = meta.Name.Trim();

            if (await _educationRepo.ExistsNameAsync(name, id))
                return new ActionResultResponse(-1, $"Trình độ học vấn \"{name}\" đã tồn tại.");

            var entity = new EducationLevel
            {
                Id = id,
                Name = name,
                Description = meta.Description?.Trim(),
                Order = meta.Order,
                UpdatedAt = DateTime.Now
            };

            var result = await _educationRepo.UpdateAsync(entity);
            return result switch
            {
                1 => new ActionResultResponse(1, "Cập nhật thành công."),
                -1 => new ActionResultResponse(-1, $"Trình độ học vấn \"{name}\" đã tồn tại."),
                _ => new ActionResultResponse(-99, "Không tìm thấy trình độ học vấn.")
            };
        }

        public async Task<ActionResultResponse> DeleteAsync(Guid id)
        {
            var result = await _educationRepo.SoftDeleteAsync(id);
            return result <= 0
                ? new ActionResultResponse(-99, "Không tìm thấy trình độ học vấn.")
                : new ActionResultResponse(1, "Xoá thành công.");
        }

        private static EducationLevelViewModel MapToViewModel(EducationLevel entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Order = entity.Order,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}
