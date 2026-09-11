using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Domain.IServices
{
    public interface IEmployeeService
    {
        Task<ActionResultResponse<List<EmployeeViewModel>>> GetListAsync();
        Task<ActionResultResponse<EmployeeViewModel>> GetDetailAsync(Guid id);
        Task<ActionResultResponse<Guid>> CreateAsync(EmployeeMeta meta);
        Task<ActionResultResponse> UpdateAsync(Guid id, EmployeeMeta meta);
        Task<ActionResultResponse> DeleteAsync(Guid id);
    }
}
