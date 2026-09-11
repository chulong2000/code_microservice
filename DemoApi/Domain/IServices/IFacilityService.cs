using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Domain.IServices
{
    public interface IFacilityService
    {
        Task<ActionResultResponse<PagedResultViewModel<FacilityViewModel>>> GetListAsync(PagingRequestMeta request, bool includeStats);
        Task<ActionResultResponse<FacilityViewModel>> GetDetailAsync(Guid id);
        Task<ActionResultResponse<Guid>> CreateAsync(FacilityMeta meta);
        Task<ActionResultResponse> UpdateAsync(Guid id, FacilityMeta meta);
        Task<ActionResultResponse> DeleteAsync(Guid id);
    }
}
