using DemoApi.Domain.ModelMetas;
using FluentValidation;

namespace DemoApi.Infrastructure.Validation
{
    public class EducationLevelMetaValidator : AbstractValidator<EducationLevelMeta>
    {
        public EducationLevelMetaValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên trình độ học vấn không được để trống.")
                .MaximumLength(100).WithMessage("Tên tối đa 100 ký tự.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Mô tả tối đa 500 ký tự.");
        }
    }
}
