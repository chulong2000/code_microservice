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
                CreatedAt = DateTime.Now,
            };

            var result = await _salaryCoefficientRepo.InsertAsync(entity);
            // Tham số thứ 3 của constructor thật là "title", không phải "data" -> phải truyền data bằng named argument.
            return new ActionResultResponse<Guid>(1, "Tạo trình độ học vấn thành công.", data: entity.Id);
        }

        public async Task<ActionResultResponse> DeleteAsync(Guid id)
        {
            var result = await _salaryCoefficientRepo.SoftDeleteAsync(id);

            return result <= 0 
                ? new ActionResultResponse(-99, "Không tìm thấy hệ số lương.")
                : new ActionResultResponse(1, "Xóa thành công.");
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

            return entity is null 
                ? new ActionResultResponse<EducationLevelSalaryCoefficientViewModel>(-99, "Không tìm hệ số lương.")
                : new ActionResultResponse<EducationLevelSalaryCoefficientViewModel>(EducationLevelSalaryCoefficientMappper.MapToViewModel(entity));
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

            return result <= 0 
                ? new ActionResultResponse(-90, "Không tìm thấy dữ liệu hệ số lương cho vị trí này.")
                : new ActionResultResponse(1, "Cập nhập thành công.");
        }
    }
}
