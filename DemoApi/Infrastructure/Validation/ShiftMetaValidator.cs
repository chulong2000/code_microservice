using DemoApi.Domain.ModelMetas;
using FluentValidation;

namespace DemoApi.Infrastructure.Validation
{
    public class ShiftMetaValidator : AbstractValidator<ShiftMeta>
    {
        public ShiftMetaValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên ca làm việc không được để trống.")
                .MaximumLength(100).WithMessage("Tên tối đa 100 ký tự.");

            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime).WithMessage("Giờ kết thúc phải sau giờ bắt đầu.");
        }
    }
}
