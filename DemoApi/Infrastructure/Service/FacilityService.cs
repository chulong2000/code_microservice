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

        public async Task<ActionResultResponse<PagedResultViewModel<FacilityViewModel>>> GetListAsync(PagingRequestMeta request, bool includeStats)
        {
            var (entities, totalRecords) = await _facilityRepo.SelectListAsync(request);

            // map then set stats by assigning the whole tuple back to the property
            var items = entities
                .Select(FacilityMapper.MapToViewModel)
                .ToList();

            if (includeStats)
            {
                foreach (var x in items)
                {
                    var employeeCount = await _facilityRepo.GetCountAllEmployeeOFFacility(x.Id);
                    var shiftCount = await _facilityRepo.GetCountAllShiftOFFacility(x.Id);
                    x.EmployeeCount = employeeCount;
                    x.ShiftCount = shiftCount;
                }
            }

            Console.WriteLine("Check_23444: " + items);

            var data = new PagedResultViewModel<FacilityViewModel>
            {
                Items = items,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = totalRecords
            };

            return new ActionResultResponse<PagedResultViewModel<FacilityViewModel>>(data);
        }

        public async Task<ActionResultResponse<FacilityViewModel>> GetDetailAsync(Guid id)
        {
            

            var entity = await _facilityRepo.SelectByIdAsync(id);

            if (entity != null)
            {
                var employeeCount = await _facilityRepo.GetCountAllEmployeeOFFacility(entity.Id);
                var shiftCount = await _facilityRepo.GetCountAllShiftOFFacility(entity.Id);
                var viewModel = FacilityMapper.MapToViewModel(entity);
                viewModel.EmployeeCount = employeeCount;
                viewModel.ShiftCount = shiftCount;
                return new ActionResultResponse<FacilityViewModel>(viewModel);
            }
            return new ActionResultResponse<FacilityViewModel>(-99, "Không tìm thấy cơ sở.");
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

            var employeeCount = await _facilityRepo.GetCountAllEmployeeOFFacility(id);
            var workScheduleCount = await _facilityRepo.GetCountAllWorkScheduleOFFacility(id);
            var shiftCount = await _facilityRepo.GetCountAllShiftOFFacility(id);

            if (employeeCount > 0 || shiftCount > 0 || workScheduleCount > 0)
            {
                return new ActionResultResponse(2, "Không thể xoá cơ sở đang có nhân viên hoặc lịch làm việc", 
                                                   "FACILITY_HAS_DEPENDENCIES");
            }

            return result switch
            {
                1 => new ActionResultResponse(1, "Xóa thành công."),
                _ => new ActionResultResponse(-99, "Không tìm thấy cơ sở.")
            };
        }
    }
}
