using DemoApi.Domain.ModelMetas;
using FluentValidation;

namespace DemoApi.Infrastructure.Validation
{
    public class FacilityMetaValidator : AbstractValidator<FacilityMeta>
    {
        public FacilityMetaValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên cơ sở không được để trống.")
                .MaximumLength(150).WithMessage("Tên tối đa 150 ký tự.");

            RuleFor(x => x.Address)
                .MaximumLength(300).WithMessage("Địa chỉ tối đa 300 ký tự.");
        }
    }
}
