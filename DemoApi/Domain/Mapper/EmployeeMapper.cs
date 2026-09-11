using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;

namespace DemoApi.Domain.Mapper
{
    public class EmployeeMapper
    {
        public static EmployeeViewModel MapToViewModel(Employee entity) => new()
        {
            Id = entity.Id,
            EmployeeCode = entity.EmployeeCode,
            JobApplicationId = entity.JobApplicationId,
            JobPositionId = entity.JobPositionId,
            PrimaryFacilityId = entity.PrimaryFacilityId,
            FullName = entity.FullName,
            Email = entity.Email,
            PhoneNumber = entity.PhoneNumber,
            DateOfBirth = entity.DateOfBirth,
            Gender = entity.Gender,
            HireDate = entity.HireDate,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }
}
