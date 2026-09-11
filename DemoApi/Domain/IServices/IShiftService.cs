using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Domain.IServices
{
    public interface IShiftService
    {
        Task<ActionResultResponse<List<ShiftViewModel>>> GetListAsync();
        Task<ActionResultResponse<List<ShiftViewModel>>> GetDetailAsync(Guid id);
        Task<ActionResultResponse<Guid>> CreateAsync(ShiftMeta meta);
        Task<ActionResultResponse> UpdateAsync(Guid id, ShiftMeta meta);
        Task<ActionResultResponse> DeleteAsync(Guid id);
    }
}
