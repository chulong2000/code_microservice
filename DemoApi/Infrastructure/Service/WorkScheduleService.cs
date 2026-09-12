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

        public async Task<ActionResultResponse<WorkScheduleBulkCreateResultViewModel>> BulkCreateMonthlyAsync(WorkScheduleBulkCreateMeta meta)
        {
            var employeeIds = meta.EmployeeIds.Distinct().ToList();
            var daysInMonth = DateTime.DaysInMonth(meta.Year, meta.Month);
            var workDates = Enumerable.Range(1, daysInMonth)
                .Select(day => new DateTime(meta.Year, meta.Month, day))
                .Where(date => meta.WorkingDaysOfWeek is not { Count: > 0 } || meta.WorkingDaysOfWeek.Contains(date.DayOfWeek))
                .ToList();

            if (workDates.Count == 0)
                return new ActionResultResponse<WorkScheduleBulkCreateResultViewModel>(-99, "Không có ngày làm việc nào phù hợp trong tháng đã chọn.");

            var now = DateTime.Now;
            var status = meta.Status.Trim();
            var note = meta.Note?.Trim();

            var entities = employeeIds
                .SelectMany(employeeId => workDates.Select(workDate => new WorkSchedule
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = employeeId,
                    ShiftId = meta.ShiftId,
                    FacilityId = meta.FacilityId,
                    WorkDate = workDate,
                    Status = status,
                    Note = note,
                    CreatedBy = meta.CreatedBy,
                    CreatedAt = now,
                }))
                .ToList();

            var totalCreated = await _workScheduleRepo.BulkInsertAsync(entities);

            var data = new WorkScheduleBulkCreateResultViewModel
            {
                TotalRequested = entities.Count,
                TotalCreated = totalCreated,
                TotalSkipped = entities.Count - totalCreated,
            };

            return new ActionResultResponse<WorkScheduleBulkCreateResultViewModel>(1, "Tạo lịch làm việc hàng loạt thành công.", data: data);
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
