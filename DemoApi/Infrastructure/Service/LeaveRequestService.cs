using DemoApi.Domain.IRepository;
using DemoApi.Domain.IServices;
using DemoApi.Domain.Mapper;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Infrastructure.Service
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepo;

        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepo)
        {
            _leaveRequestRepo = leaveRequestRepo;
        }

        public async Task<ActionResultResponse<List<LeaveRequestViewModel>>> GetListAsync()
        {
            var entities = await _leaveRequestRepo.SelectAllAsync();
            var data = entities.Select(LeaveRequestMapper.MapToViewModel).ToList();

            return new ActionResultResponse<List<LeaveRequestViewModel>>(data);
        }

        public async Task<ActionResultResponse<LeaveRequestViewModel>> GetDetailAsync(Guid id)
        {
            var entity = await _leaveRequestRepo.SelectByIdAsync(id);
            return entity is null
                   ? new ActionResultResponse<LeaveRequestViewModel>(-99, "Không tìm thấy đơn nghỉ phép.")
                   : new ActionResultResponse<LeaveRequestViewModel>(LeaveRequestMapper.MapToViewModel(entity));
        }

        public async Task<ActionResultResponse<Guid>> CreateAsync(LeaveRequestMeta meta)
        {
            var entity = new LeaveRequest
            {
                Id = Guid.NewGuid(),
                EmployeeId = meta.EmployeeId,
                LeaveType = meta.LeaveType.Trim(),
                FromDate = meta.FromDate,
                ToDate = meta.ToDate,
                Reason = meta.Reason?.Trim(),
                Status = meta.Status.Trim(),
                ApprovedBy = meta.ApprovedBy,
                ApprovedAt = meta.ApprovedAt,
                CreatedAt = DateTime.Now,
            };

            var result = await _leaveRequestRepo.InsertAsync(entity);

            return result == 1
                   ? new ActionResultResponse<Guid>(1, "Tạo mới thành công.", data: entity.Id)
                   : new ActionResultResponse<Guid>(-99, "Tạo mới thất bại.");
        }

        public async Task<ActionResultResponse> UpdateAsync(Guid id, LeaveRequestMeta meta)
        {
            var entity = new LeaveRequest
            {
                Id = id,
                EmployeeId = meta.EmployeeId,
                LeaveType = meta.LeaveType.Trim(),
                FromDate = meta.FromDate,
                ToDate = meta.ToDate,
                Reason = meta.Reason?.Trim(),
                Status = meta.Status.Trim(),
                ApprovedBy = meta.ApprovedBy,
                ApprovedAt = meta.ApprovedAt,
                UpdatedAt = DateTime.Now
            };

            var result = await _leaveRequestRepo.UpdateAsync(entity);
            return result == 1
                   ? new ActionResultResponse(1, "Cập nhật thành công.")
                   : new ActionResultResponse(-99, "Không tìm thấy đơn nghỉ phép.");
        }

        public async Task<ActionResultResponse> DeleteAsync(Guid id)
        {
            var result = await _leaveRequestRepo.SoftDeleteAsync(id);
            return result == 1
                   ? new ActionResultResponse(1, "Xóa thành công.")
                   : new ActionResultResponse(-99, "Không tìm thấy đơn nghỉ phép.");
        }
    }
}
