using DemoApi.Domain.IRepository;
using DemoApi.Domain.IServices;
using DemoApi.Domain.Mapper;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Infrastructure.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepo;

        public EmployeeService(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        public async Task<ActionResultResponse<List<EmployeeViewModel>>> GetListAsync(Guid? facilityId, Guid? jobPositionId, string? status)
        {
            var entities = await _employeeRepo.SelectAllAsync(facilityId, jobPositionId, status);
            var data = entities.Select(EmployeeMapper.MapToViewModel).ToList();

            return new ActionResultResponse<List<EmployeeViewModel>>(data);
        }

        public async Task<ActionResultResponse<EmployeeViewModel>> GetDetailAsync(Guid id)
        {
            var entity = await _employeeRepo.SelectByIdAsync(id);
            return entity is null
                   ? new ActionResultResponse<EmployeeViewModel>(-99, "Không tìm thấy nhân viên.")
                   : new ActionResultResponse<EmployeeViewModel>(EmployeeMapper.MapToViewModel(entity));
        }

        public async Task<ActionResultResponse<Guid>> CreateAsync(EmployeeMeta meta)
        {
            var entity = new Employee
            {
                Id = Guid.NewGuid(),
                EmployeeCode = meta.EmployeeCode.Trim(),
                JobApplicationId = meta.JobApplicationId,
                JobPositionId = meta.JobPositionId,
                PrimaryFacilityId = meta.PrimaryFacilityId,
                FullName = meta.FullName.Trim(),
                Email = meta.Email?.Trim(),
                PhoneNumber = meta.PhoneNumber?.Trim(),
                DateOfBirth = meta.DateOfBirth,
                Gender = meta.Gender?.Trim(),
                HireDate = meta.HireDate,
                Status = meta.Status.Trim(),
                CreatedAt = DateTime.Now,
            };

            var result = await _employeeRepo.InsertAsync(entity);

            return result == 1
                   ? new ActionResultResponse<Guid>(1, "Tạo mới thành công.", data: entity.Id)
                   : new ActionResultResponse<Guid>(-99, "Tạo mới thất bại.");
        }

        public async Task<ActionResultResponse> UpdateAsync(Guid id, EmployeeMeta meta)
        {
            var entity = new Employee
            {
                Id = id,
                EmployeeCode = meta.EmployeeCode.Trim(),
                JobApplicationId = meta.JobApplicationId,
                JobPositionId = meta.JobPositionId,
                PrimaryFacilityId = meta.PrimaryFacilityId,
                FullName = meta.FullName.Trim(),
                Email = meta.Email?.Trim(),
                PhoneNumber = meta.PhoneNumber?.Trim(),
                DateOfBirth = meta.DateOfBirth,
                Gender = meta.Gender?.Trim(),
                HireDate = meta.HireDate,
                Status = meta.Status.Trim(),
                UpdatedAt = DateTime.Now
            };

            var result = await _employeeRepo.UpdateAsync(entity);
            return result == 1
                   ? new ActionResultResponse(1, "Cập nhật thành công.")
                   : new ActionResultResponse(-99, "Không tìm thấy nhân viên.");
        }

        public async Task<ActionResultResponse> DeleteAsync(Guid id)
        {
            var result = await _employeeRepo.SoftDeleteAsync(id);
            return result == 1
                   ? new ActionResultResponse(1, "Xóa thành công.")
                   : new ActionResultResponse(-99, "Không tìm thấy nhân viên.");
        }
    }
}
