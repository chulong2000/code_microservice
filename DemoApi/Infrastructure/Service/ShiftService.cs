using DemoApi.Domain.IRepository;
using DemoApi.Domain.IServices;
using DemoApi.Domain.Mapper;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Infrastructure.Service
{
    public class ShiftService : IShiftService
    {
        private readonly IShiftRepository _shiftRepo;

        public ShiftService(IShiftRepository shiftRepo)
        {
            _shiftRepo = shiftRepo;
        }

        public async Task<ActionResultResponse<List<ShiftViewModel>>> GetListAsync()
        {
            var entities = await _shiftRepo.SelectAllAsync();
            var data = entities.Select(ShiftMapper.MapToViewModel).ToList();

            return new ActionResultResponse<List<ShiftViewModel>>(data);
        }

        public async Task<ActionResultResponse<List<ShiftViewModel>>> GetDetailAsync(Guid id)
        {
            var entity = await _shiftRepo.SelectByIdAsync(id);
            return entity is null
                   ? new ActionResultResponse<List<ShiftViewModel>>(-99, "Không tìm thấy ca làm việc.")
                   : new ActionResultResponse<List<ShiftViewModel>>(entity.Select(ShiftMapper.MapToViewModel).ToList());
        }

        public async Task<ActionResultResponse<Guid>> CreateAsync(ShiftMeta meta)
        {
            var entity = new Shift
            {
                Id = Guid.NewGuid(),
                FacilityId = meta.FacilityId,
                Name = meta.Name.Trim(),
                StartTime = meta.StartTime,
                EndTime = meta.EndTime,
                CreatedAt = DateTime.Now,
            };

            var result = await _shiftRepo.InsertAsync(entity);

            return result == 1
                   ? new ActionResultResponse<Guid>(1, "Tạo mới thành công.", data: entity.Id)
                   : new ActionResultResponse<Guid>(-99, "Tạo mới thất bại.");
        }

        public async Task<ActionResultResponse> UpdateAsync(Guid id, ShiftMeta meta)
        {
            var entity = new Shift
            {
                Id = id,
                FacilityId = meta.FacilityId,
                Name = meta.Name.Trim(),
                StartTime = meta.StartTime,
                EndTime = meta.EndTime,
                UpdatedAt = DateTime.Now
            };

            var result = await _shiftRepo.UpdateAsync(entity);
            return result == 1
                   ? new ActionResultResponse(1, "Cập nhật thành công.")
                   : new ActionResultResponse(-99, "Không tìm thấy ca làm việc.");
        }

        public async Task<ActionResultResponse> DeleteAsync(Guid id)
        {
            var result = await _shiftRepo.SoftDeleteAsync(id);
            return result == 1
                   ? new ActionResultResponse(1, "Xóa thành công.")
                   : new ActionResultResponse(-99, "Không tìm thấy ca làm việc.");
        }
    }
}
