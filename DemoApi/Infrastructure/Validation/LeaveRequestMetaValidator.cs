using DemoApi.Domain.ModelMetas;
using FluentValidation;

namespace DemoApi.Infrastructure.Validation
{
    public class LeaveRequestMetaValidator : AbstractValidator<LeaveRequestMeta>
    {
        public LeaveRequestMetaValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("Nhân viên không được để trống.");

            RuleFor(x => x.LeaveType)
                .NotEmpty().WithMessage("Loại nghỉ phép không được để trống.")
                .MaximumLength(30).WithMessage("Loại nghỉ phép tối đa 30 ký tự.");

            RuleFor(x => x.ToDate)
                .GreaterThanOrEqualTo(x => x.FromDate).WithMessage("Ngày kết thúc phải sau hoặc bằng ngày bắt đầu.");

            RuleFor(x => x.Reason)
                .MaximumLength(300).WithMessage("Lý do tối đa 300 ký tự.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Trạng thái không được để trống.")
                .MaximumLength(20).WithMessage("Trạng thái tối đa 20 ký tự.");
        }
    }
}
