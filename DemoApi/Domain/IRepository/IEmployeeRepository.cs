using DemoApi.Domain.Models;

namespace DemoApi.Domain.IRepository
{
    public interface IEmployeeRepository
    {
        Task<int> InsertAsync(Employee entity);
        Task<int> UpdateAsync(Employee entity);
        Task<int> SoftDeleteAsync(Guid id);
        Task<List<Employee>> SelectAllAsync(Guid? facilityId, Guid? jobPositionId, string? status);
        Task<Employee?> SelectByIdAsync(Guid id);
    }
}
