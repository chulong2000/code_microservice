using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.Domain.IServices
{
    public interface IEmployeeService
    {
        Task<ActionResultResponse<List<EmployeeViewModel>>> GetListAsync(Guid? facilityId, Guid? jobPositionId, string? status);
        Task<ActionResultResponse<EmployeeViewModel>> GetDetailAsync(Guid id);
        Task<ActionResultResponse<Guid>> CreateAsync(EmployeeMeta meta);
        Task<ActionResultResponse> UpdateAsync(Guid id, EmployeeMeta meta);
        Task<ActionResultResponse> DeleteAsync(Guid id);
    }
}
