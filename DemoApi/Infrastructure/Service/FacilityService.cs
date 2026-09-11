using DemoApi.Domain.IRepository;
using DemoApi.Domain.IServices;
using DemoApi.Domain.Mapper;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Infrastructure.Service
{
    public class FacilityService : IFacilityService
    {
        private readonly IFacilityRepository _facilityRepo;

        public FacilityService(IFacilityRepository facilityRepo)
        {
            _facilityRepo = facilityRepo;
        }

        public async Task<ActionResultResponse<PagedResultViewModel<FacilityViewModel>>> GetListAsync(PagingRequestMeta request)
        {
            var (entities, totalRecords) = await _facilityRepo.SelectListAsync(request);

            var data = new PagedResultViewModel<FacilityViewModel>
            {
                Items = entities.Select(FacilityMapper.MapToViewModel).ToList(),
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = totalRecords
            };

            return new ActionResultResponse<PagedResultViewModel<FacilityViewModel>>(data);
        }

        public async Task<ActionResultResponse<FacilityViewModel>> GetDetailAsync(Guid id)
        {
            var entity = await _facilityRepo.SelectByIdAsync(id);
            return entity is null
                   ? new ActionResultResponse<FacilityViewModel>(-99, "Không tìm thấy cơ sở.")
                   : new ActionResultResponse<FacilityViewModel>(FacilityMapper.MapToViewModel(entity));
        }

        public async Task<ActionResultResponse<Guid>> CreateAsync(FacilityMeta meta)
        {
            var name = meta.Name.Trim();
            var entity = new Facility
            {
                Id = Guid.NewGuid(),
                Name = name,
                Address = meta.Address?.Trim(),
                CreatedAt = DateTime.Now,
            };

            var result = await _facilityRepo.InsertAsync(entity);

            return result switch
            {
                1 => new ActionResultResponse<Guid>(1, "Tạo mới thành công.", data: entity.Id),
                -1 => new ActionResultResponse<Guid>(-1, $"Cơ sở \"{name}\" đã tồn tại."),
                _ => new ActionResultResponse<Guid>(-99, "Không tìm thấy cơ sở.")
            };
        }

        public async Task<ActionResultResponse> UpdateAsync(Guid id, FacilityMeta meta)
        {
            var name = meta.Name.Trim();

            var entity = new Facility
            {
                Id = id,
                Name = name,
                Address = meta.Address?.Trim(),
                UpdatedAt = DateTime.Now
            };

            var result = await _facilityRepo.UpdateAsync(entity);
            return result switch
            {
                1 => new ActionResultResponse(1, "Cập nhật thành công."),
                -1 => new ActionResultResponse(-1, $"Cơ sở \"{name}\" đã tồn tại."),
                _ => new ActionResultResponse(-99, "Không tìm thấy cơ sở.")
            };
        }

        public async Task<ActionResultResponse> DeleteAsync(Guid id)
        {
            var result = await _facilityRepo.SoftDeleteAsync(id);
            return result switch
            {
                1 => new ActionResultResponse(1, "Xóa thành công."),
                _ => new ActionResultResponse(-99, "Không tìm thấy cơ sở.")
            };
        }
    }
}
