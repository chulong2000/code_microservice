using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using FluentValidation;

namespace DemoApi.Infrastructure.Validation
{
    public class EducationLevelSalaryCoefficientValidator : AbstractValidator<EducationLevelSalaryCoefficientMeta>
    {
        public EducationLevelSalaryCoefficientValidator()
        {

            RuleFor(x => x.BaseCoefficient)
                        .GreaterThan(0).WithMessage("Hệ số lương cơ bản phải lớn hơn 0");

             RuleFor(x => x.Notes)
                    .MaximumLength(500).WithMessage("Ghi chú tối đa 500 ký tự.");
           
        }
    }
}
