using DemoApi.Domain.ModelMetas;
using FluentValidation;

namespace DemoApi.Infrastructure.Validation
{
    public class JobPositionMetaValidation : AbstractValidator<JobPositionMeta>
    {
        public JobPositionMetaValidation()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tên vị trí công việc không được để trống")
                .MaximumLength(100).WithMessage("Tên tối đa 100 ký tự.");

            RuleFor(x => x.Department)
                .NotEmpty().WithMessage("Tên khoa tuyển không được để trống")
                .MaximumLength(500).WithMessage("Mô tả tối đa 500 ký tự.");

            RuleFor(x => x.OpenSlots)
                        .NotNull().WithMessage("Số lượng cần tuyển không được để trống")
                        .GreaterThan(0).WithMessage("Số lượng cần tuyển phải lớn hơn 0")
                        .LessThanOrEqualTo(15).WithMessage("Số lượng cần tuyển không được vượt quá 15");
        }
    }
}
