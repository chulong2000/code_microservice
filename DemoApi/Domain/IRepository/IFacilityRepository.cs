using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;

namespace DemoApi.Domain.IRepository
{
    public interface IFacilityRepository
    {
        Task<bool> ExistsNameAsync(string name, Guid? excludeId);
        Task<int> InsertAsync(Facility entity);
        Task<int> UpdateAsync(Facility entity);
        Task<int> SoftDeleteAsync(Guid id);
        Task<(List<Facility> Items, int TotalRecords)> SelectListAsync(PagingRequestMeta request);
        Task<Facility?> SelectByIdAsync(Guid id);
    }
}
