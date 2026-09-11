using DemoApi.Domain.Models;

namespace DemoApi.Domain.IRepository
{
    public interface IShiftRepository
    {
        Task<int> InsertAsync(Shift entity);
        Task<int> UpdateAsync(Shift entity);
        Task<int> SoftDeleteAsync(Guid id);
        Task<List<Shift>> SelectAllAsync();
        Task<List<Shift>> SelectByIdAsync(Guid id);
    }
}
