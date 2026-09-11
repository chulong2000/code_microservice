using DemoApi.Domain.ModelMetas;
using FluentValidation;

namespace DemoApi.Infrastructure.Validation
{
    public class WorkScheduleMetaValidator : AbstractValidator<WorkScheduleMeta>
    {
        public WorkScheduleMetaValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("Nhân viên không được để trống.");

            RuleFor(x => x.ShiftId)
                .NotEmpty().WithMessage("Ca làm việc không được để trống.");

            RuleFor(x => x.FacilityId)
                .NotEmpty().WithMessage("Cơ sở làm việc không được để trống.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Trạng thái không được để trống.")
                .MaximumLength(20).WithMessage("Trạng thái tối đa 20 ký tự.");

            RuleFor(x => x.Note)
                .MaximumLength(300).WithMessage("Ghi chú tối đa 300 ký tự.");
        }
    }
}
