using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;

namespace DemoApi.Domain.Mapper
{
    public class JobApplicationMapper
    {

        public static JobApplicationViewModel MapToViewModel(JobApplication entity) => new()
        {
            Id = entity.Id,
            FullName = entity.FullName,
            Email = entity.Email,
            CoverLetter = entity.CoverLetter,
            JobPosition = entity.JobPosition,
            Gender = entity.Gender,
            DateOfBirth = entity.DateOfBirth,
            CvFileUrl = entity.CvFileUrl,
            PhoneNumber = entity.PhoneNumber,
            AppliedAt = entity.AppliedAt,
        };
    }
}
