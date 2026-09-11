using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Domain.IServices
{
    public interface ILeaveRequestService
    {
        Task<ActionResultResponse<List<LeaveRequestViewModel>>> GetListAsync();
        Task<ActionResultResponse<LeaveRequestViewModel>> GetDetailAsync(Guid id);
        Task<ActionResultResponse<Guid>> CreateAsync(LeaveRequestMeta meta);
        Task<ActionResultResponse> UpdateAsync(Guid id, LeaveRequestMeta meta);
        Task<ActionResultResponse> DeleteAsync(Guid id);
    }
}
