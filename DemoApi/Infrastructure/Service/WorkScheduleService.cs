using DemoApi.Domain.IRepository;
using DemoApi.Domain.IServices;
using DemoApi.Domain.Mapper;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;
using DemoApi.Infrastructure.Data;
using GHM.Infrastructure.Models;

namespace DemoApi.Infrastructure.Service
{
    public class WorkScheduleService : IWorkScheduleService
    {
        private readonly IWorkScheduleRepository _workScheduleRepo;
        private readonly ILeaveRequestRepository _leaveRequestRepo;
        private readonly IDbSession _dbSession;

        public WorkScheduleService(IWorkScheduleRepository workScheduleRepo, ILeaveRequestRepository leaveRequestRepo, IDbSession dbSession)
        {
            _workScheduleRepo = workScheduleRepo;
            _leaveRequestRepo = leaveRequestRepo;
            _dbSession = dbSession;
        }

        public async Task<ActionResultResponse<List<WorkScheduleViewModel>>> GetListAsync(Guid? employeeId, Guid? facilityId, DateTime? fromDate, DateTime? toDate, Guid? shiftId)
        {
            var entities = await _workScheduleRepo.SelectByFilterAsync(employeeId, facilityId, fromDate, toDate, shiftId);
            var data = entities.Select(WorkScheduleMapper.MapToViewModel).ToList();

            return new ActionResultResponse<List<WorkScheduleViewModel>>(data);
        }

        public async Task<ActionResultResponse<List<WorkScheduleViewModel>>> GetUpcomingByEmployeeAsync(Guid employeeId)
        {
            var entities = await _workScheduleRepo.SelectByFilterAsync(employeeId, null, DateTime.Today, null, null);

            // Chỉ tính là "sắp tới" khi ca đã được công bố cho nhân viên; Draft (nội bộ, chưa công bố)
            // và Cancelled/Rejected (không còn hiệu lực) không nằm trong danh sách này.
            var upcoming = entities
                .Where(w => w.Status == "Confirmed")
                .ToList();

            var data = upcoming.Select(WorkScheduleMapper.MapToViewModel).ToList();
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
            var conflict = await _leaveRequestRepo.SelectConflictByDateAsync(meta.EmployeeId, meta.WorkDate);
            if (conflict is not null)
            {
                return new ActionResultResponse<Guid>(-99, $"Nhân viên đã có đơn nghỉ phép ({conflict.Status}) trong ngày này, không thể tạo lịch làm việc.");
            }

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
            Guid facilityId = meta.FacilityID!.Value;
            var entries = meta.Entries;

            // Lấy 1 lần toàn bộ đơn nghỉ phép (Pending/Approved) của các nhân viên xuất hiện trong entries,
            // trong khoảng ngày nhỏ nhất-lớn nhất của cả batch, để tránh N+1 query khi lọc từng entry.
            var employeeIds = entries.Select(e => e.EmployeeId).Distinct().ToList();
            var scopeFromDate = entries.Min(e => e.WorkDate);
            var scopeToDate = entries.Max(e => e.WorkDate);
            var leaveConflicts = await _leaveRequestRepo.SelectConflictsInScopeAsync(employeeIds, scopeFromDate, scopeToDate);

            var leaveConflictSkipped = new List<WorkScheduleLeaveConflictItem>();
            var validEntries = new List<WorkScheduleEntryRequest>();

            foreach (var entry in entries)
            {
                var conflict = leaveConflicts.FirstOrDefault(lr => lr.EmployeeId == entry.EmployeeId
                                                                 && entry.WorkDate.Date >= lr.FromDate.Date
                                                                 && entry.WorkDate.Date <= lr.ToDate.Date);
                if (conflict is not null)
                {
                    leaveConflictSkipped.Add(new WorkScheduleLeaveConflictItem
                    {
                        EmployeeId = entry.EmployeeId,
                        WorkDate = entry.WorkDate,
                        LeaveStatus = conflict.Status,
                    });
                }
                else
                {
                    validEntries.Add(entry);
                }
            }

            var entities = validEntries
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

            var createdCount = entities.Count > 0 ? await _workScheduleRepo.BulkInsertAsync(entities) : 0;

            var data = new WorkScheduleBulkCreateResultViewModel
            {
                TotalRequested = entries.Count,
                CreatedCount = createdCount,
                DuplicateScheduleSkippedCount = entities.Count - createdCount,
                LeaveConflictSkipped = leaveConflictSkipped,
            };

            if (createdCount == 0)
                return new ActionResultResponse<WorkScheduleBulkCreateResultViewModel>(-99, "Không có lịch làm việc nào được tạo mới.", data: data);
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

        public async Task<ActionResultResponse<WorkScheduleConflictViewModel>> CheckLeaveConflictAsync(Guid employeeId, DateTime workDate)
        {
            var conflict = await _leaveRequestRepo.SelectConflictByDateAsync(employeeId, workDate);

            var data = new WorkScheduleConflictViewModel
            {
                HasConflict = conflict is not null,
                LeaveRequestId = conflict?.Id,
                LeaveType = conflict?.LeaveType,
                FromDate = conflict?.FromDate,
                ToDate = conflict?.ToDate,
                Status = conflict?.Status,
            };

            return new ActionResultResponse<WorkScheduleConflictViewModel>(data);
        }

        public async Task<ActionResultResponse> BulkSyncAsync(Guid facilityId, DateTime fromDate, DateTime toDate, WorkScheduleBulkSyncMeta meta)
        {
            var existingDraftRows = await _workScheduleRepo.SelectDraftInScopeAsync(facilityId, fromDate, toDate);
            var entries = meta.Entries;

            var toDelete = existingDraftRows
                .Where(r => !entries.Any(e => e.EmployeeId == r.EmployeeId
                                              && e.WorkDate.Date == r.WorkDate.Date
                                              && e.ShifId == r.ShiftId
                                            ))
                .Select(r => r.Id)
                .ToList();

            Console.WriteLine($"Existing Draft Rows Count: {toDelete}");

            var toInsert = entries
                .Where(e => !existingDraftRows.Any(r => r.EmployeeId == e.EmployeeId
                                                      && r.WorkDate.Date == e.WorkDate.Date
                                                      && r.ShiftId == e.ShifId))
                .Select(e => new WorkSchedule
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = e.EmployeeId,
                    ShiftId = e.ShifId,
                    FacilityId = facilityId,
                    WorkDate = e.WorkDate,
                    Status = "Draft",
                    Note = e.Note?.Trim(),
                    CreatedBy = e.CreatBy,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                })
                .ToList();

            Console.WriteLine($"Existing Draft Rows Count: {toInsert}");

            await _dbSession.BeginTransactionAsync();
            try
            {
                if (toDelete.Count > 0)
                {
                    await _workScheduleRepo.BulkSoftDeleteAsync(toDelete);
                }

                if (toInsert.Count > 0)
                {
                    await _workScheduleRepo.BulkInsertAsync(toInsert);
                }

                await _dbSession.CommitAsync();
            }
            catch
            {
                await _dbSession.RollbackAsync();
                throw;
            }

            return new ActionResultResponse(1, $"Đồng bộ thành công: thêm {toInsert.Count}, xoá {toDelete.Count}, giữ nguyên {entries.Count - toInsert.Count}.");
        }

        public async Task<ActionResultResponse> PublishBatchAsync(Guid facilityId, DateTime fromDate, DateTime toDate, Guid? publishedBy)
        {
            var affected = await _workScheduleRepo.PublishBatchAsync(facilityId, fromDate, toDate, publishedBy, DateTime.Now);

            return affected > 0
                   ? new ActionResultResponse(1, $"Đã công bố {affected} ca làm việc.")
                   : new ActionResultResponse(-99, "Không có ca nào ở trạng thái Draft trong phạm vi này để công bố.");
        }

        public async Task<ActionResultResponse> ConfirmAsync(Guid id, WorkScheduleConfirmMeta meta)
        {
            var result = await _workScheduleRepo.ConfirmAsync(id, meta.ConfirmedBy, DateTime.Now);
            return result == 1
                   ? new ActionResultResponse(1, "Xác nhận lịch làm việc thành công.")
                   : new ActionResultResponse(-99, "Không tìm thấy lịch làm việc, lịch chưa được công bố, hoặc không thuộc về nhân viên này.");
        }
    }
}
