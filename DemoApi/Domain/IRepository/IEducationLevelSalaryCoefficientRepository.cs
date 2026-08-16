using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;

namespace DemoApi.Domain.IRepository
{
    public interface IEducationLevelSalaryCoefficientRepository
    {
        Task<EducationLevelSalaryCoefficient> GetSalaryCoefficientByEducationLevelId(Guid id);
        Task<List<EducationLevelSalaryCoefficient>> SelectListAsync();
        Task<int> SoftDeleteAsync(Guid id);
        Task<int> UpsertAsync(EducationLevelSalaryCoefficient entity);
    }
}
