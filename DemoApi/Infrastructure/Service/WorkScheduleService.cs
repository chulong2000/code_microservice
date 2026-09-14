using DemoApi.Domain.IRepository;
using DemoApi.Domain.IServices;
using DemoApi.Domain.Mapper;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Infrastructure.Service
{
    public class WorkScheduleService : IWorkScheduleService
    {
        private readonly IWorkScheduleRepository _workScheduleRepo;

        public WorkScheduleService(IWorkScheduleRepository workScheduleRepo)
        {
            _workScheduleRepo = workScheduleRepo;
        }

        public async Task<ActionResultResponse<List<WorkScheduleViewModel>>> GetListAsync()
        {
            var entities = await _workScheduleRepo.SelectAllAsync();
            var data = entities.Select(WorkScheduleMapper.MapToViewModel).ToList();

            return new ActionResultResponse<List<WorkScheduleViewModel>>(data);
        }

        public async Task<ActionResultResponse<WorkScheduleViewModel>> GetDetailAsync(Guid id)
        {
            var entity = await _workScheduleRepo.SelectByIdAsync(id);
            return entity is null
                   ? new ActionResultResponse<WorkScheduleViewModel>(-99, "Không tìm thấy lịch làm việc.")
                   : new ActionResultResponse<WorkScheduleViewModel>(WorkScheduleMapper.MapToViewModel(entity));
        }

        public async Task<ActionResultResponse<Guid>> CreateAsync(WorkScheduleMeta meta)
        {
            var entity = new WorkSchedule
            {
                Id = Guid.NewGuid(),
                EmployeeId = meta.EmployeeId,
                ShiftId = meta.ShiftId,
                FacilityId = meta.FacilityId,
                WorkDate = meta.WorkDate,
                Status = meta.Status.Trim(),
                Note = meta.Note?.Trim(),
                CreatedBy = meta.CreatedBy,
                CreatedAt = DateTime.Now,
            };

            var result = await _workScheduleRepo.InsertAsync(entity);

            return result == 1
                   ? new ActionResultResponse<Guid>(1, "Tạo mới thành công.", data: entity.Id)
                   : new ActionResultResponse<Guid>(-99, "Tạo mới thất bại.");
        }

        public async Task<ActionResultResponse> BulkCreateMonthlyAsync(WorkScheduleBulkCreateMeta meta)
        {
            Guid facilityId = meta.FacilityID!.Value;
            var entities = meta.Entries
                           .Select(item => new WorkSchedule
                           {
                               Id = Guid.NewGuid(),
                               EmployeeId = item.EmployeeId,
                               ShiftId = item.ShifId,
                               FacilityId = facilityId,
                               WorkDate = item.WorkDate,
                               Status = "Draft",
                               Note = item.Note?.Trim(),
                               CreatedBy = item.CreatBy,
                               CreatedAt = DateTime.Now,
                               UpdatedAt = null,
                               PublishedAt = null,
                               PublishedBy = null,
                               confirmedAt = null,
                               confirmedBy = null,
                               IsDeleted = false,
                           })
                          .ToList();

            var totalCreated = await _workScheduleRepo.BulkInsertAsync(entities);

            return new ActionResultResponse(1, "Tạo lịch làm việc hàng loạt thành công.");
        }

        public async Task<ActionResultResponse> UpdateAsync(Guid id, WorkScheduleMeta meta)
        {
            var entity = new WorkSchedule
            {
                Id = id,
                EmployeeId = meta.EmployeeId,
                ShiftId = meta.ShiftId,
                FacilityId = meta.FacilityId,
                WorkDate = meta.WorkDate,
                Status = meta.Status.Trim(),
                Note = meta.Note?.Trim(),
                UpdatedAt = DateTime.Now
            };

            var result = await _workScheduleRepo.UpdateAsync(entity);
            return result == 1
                   ? new ActionResultResponse(1, "Cập nhật thành công.")
                   : new ActionResultResponse(-99, "Không tìm thấy lịch làm việc.");
        }

        public async Task<ActionResultResponse> DeleteAsync(Guid id)
        {
            var result = await _workScheduleRepo.SoftDeleteAsync(id);
            return result == 1
                   ? new ActionResultResponse(1, "Xóa thành công.")
                   : new ActionResultResponse(-99, "Không tìm thấy lịch làm việc.");
        }
    }
}
