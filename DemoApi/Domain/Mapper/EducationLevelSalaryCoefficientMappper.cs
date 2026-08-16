
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;

namespace DemoApi.Domain.Mapper
{
    public class EducationLevelSalaryCoefficientMappper
    {
        public static EducationLevelSalaryCoefficientViewModel MapToViewModel(EducationLevelSalaryCoefficient entity) => new()
        {
            Notes = entity.Notes,
            AllowancePercentage = entity.AllowancePercentage,
            BaseCoefficient = entity.BaseCoefficient,
            educationLevel = entity.educationLevel,
            EffectiveFrom = entity.EffectiveFrom
        };


    }
}
