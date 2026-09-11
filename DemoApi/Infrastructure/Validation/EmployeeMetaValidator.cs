using DemoApi.Domain.ModelMetas;
using FluentValidation;

namespace DemoApi.Infrastructure.Validation
{
    public class EmployeeMetaValidator : AbstractValidator<EmployeeMeta>
    {
        public EmployeeMetaValidator()
        {
            RuleFor(x => x.EmployeeCode)
                .NotEmpty().WithMessage("Mã nhân viên không được để trống.")
                .MaximumLength(50).WithMessage("Mã nhân viên tối đa 50 ký tự.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Họ tên không được để trống.")
                .MaximumLength(200).WithMessage("Họ tên tối đa 200 ký tự.");

            RuleFor(x => x.Email)
                .MaximumLength(150).WithMessage("Email tối đa 150 ký tự.");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20).WithMessage("Số điện thoại tối đa 20 ký tự.");

            RuleFor(x => x.Gender)
                .MaximumLength(20).WithMessage("Giới tính tối đa 20 ký tự.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Trạng thái không được để trống.")
                .MaximumLength(20).WithMessage("Trạng thái tối đa 20 ký tự.");

            RuleFor(x => x.JobPositionId)
                .NotEmpty().WithMessage("Chức danh không được để trống.");

            RuleFor(x => x.PrimaryFacilityId)
                .NotEmpty().WithMessage("Cơ sở làm việc chính không được để trống.");
        }
    }
}
