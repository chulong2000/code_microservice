using DemoApi.Domain.IRepository;
using DemoApi.Domain.IServices;
using DemoApi.Domain.Mapper;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Infrastructure.Service
{
    public class EducationLevelSalaryCoefficientService : IEducationLevelSalaryCoefficientService
    {
        private readonly IEducationLevelSalaryCoefficientRepository _salaryCoefficientRepo;

        public EducationLevelSalaryCoefficientService(IEducationLevelSalaryCoefficientRepository salaryCoefficientRepo)
        {
            _salaryCoefficientRepo = salaryCoefficientRepo;
        }

        public async Task<ActionResultResponse<Guid>> CreateAsync(EducationLevelSalaryCoefficientMeta meta)
        {
            var entity = new EducationLevelSalaryCoefficient
            {
                Id = Guid.NewGuid(),
                EducationLevelId = meta.EducationLevelId,
                BaseCoefficient = meta.BaseCoefficient,
                AllowancePercentage = meta.AllowancePercentage,
                EffectiveFrom = meta.EffectiveFrom,
                Notes = meta.Notes,
                CreatedAt = meta.CreatedAt,
            };

            var result = await _salaryCoefficientRepo.InsertAsync(entity);
            if (result <= 0)
            {
                new ActionResultResponse<Guid>(-99, "Ứng tuyển CV thất bại.");
            }
            // Tham số thứ 3 của constructor thật là "title", không phải "data" -> phải truyền data bằng named argument.
            return new ActionResultResponse<Guid>(1, "Tạo trình độ học vấn thành công.", data: entity.Id);
        }

        public async Task<ActionResultResponse> DeleteAsync(Guid id)
        {
            var result = await _salaryCoefficientRepo.SoftDeleteAsync(id);
            return result switch
            {
                1 => new ActionResultResponse(1, "Xóa thành công."),
                -1 => new ActionResultResponse(-1, "Không thể xóa do vẫn còn tồn tại dữ liệu tham chiếu"),
                _ => new ActionResultResponse(-99, "Không tìm thấy trình độ học vấn.")
            };
        }

        public async Task<ActionResultResponse<List<EducationLevelSalaryCoefficientViewModel>>> GetListAsync()
        {
            var entities = await _salaryCoefficientRepo.SelectListAsync();
            var data = entities.Select(EducationLevelSalaryCoefficientMappper.MapToViewModel).ToList();

            // Constructor (T data) tự set Code = 1 — dùng cho case thành công đơn giản, không cần message riêng.
            return new ActionResultResponse<List<EducationLevelSalaryCoefficientViewModel>>(data);
        }

        public async Task<ActionResultResponse<EducationLevelSalaryCoefficientViewModel>> GetSalaryCoefficientByEducationLevelId(Guid id)
        {
            var entity = await _salaryCoefficientRepo.GetSalaryCoefficientByEducationLevelId(id);
            if (entity is null)
                return new ActionResultResponse<EducationLevelSalaryCoefficientViewModel>(-99, "Không tìm hệ số lương.");

            return new ActionResultResponse<EducationLevelSalaryCoefficientViewModel>(EducationLevelSalaryCoefficientMappper.MapToViewModel(entity));
        }

        public async Task<ActionResultResponse> UpdateAsync(EducationLevelSalaryCoefficientMeta meta)
        {
           
            var entity = new EducationLevelSalaryCoefficient
            {
                EducationLevelId = meta.EducationLevelId,
                BaseCoefficient = meta.BaseCoefficient,
                AllowancePercentage = meta.AllowancePercentage,
                EffectiveFrom = meta.EffectiveFrom,
                Notes = meta.Notes,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            var result = await _salaryCoefficientRepo.UpdateAsync(entity);
            if (result <= 0)
                return new ActionResultResponse(-90, "Thêm mới/cập nhập không thành công.");
            return new ActionResultResponse(1, "Thêm mới/cập nhập thành công.");
        }
    }
}
