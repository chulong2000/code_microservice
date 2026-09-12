using DemoApi.Domain.ModelMetas;
using FluentValidation;

namespace DemoApi.Infrastructure.Validation
{
    public class WorkScheduleBulkCreateMetaValidator : AbstractValidator<WorkScheduleBulkCreateMeta>
    {
        public WorkScheduleBulkCreateMetaValidator()
        {
            RuleFor(x => x.EmployeeIds)
                .NotEmpty().WithMessage("Danh sách nhân viên không được để trống.");

            RuleForEach(x => x.EmployeeIds)
                .NotEmpty().WithMessage("Id nhân viên không hợp lệ.");

            RuleFor(x => x.Year)
                .InclusiveBetween(2000, 2100).WithMessage("Năm không hợp lệ.");

            RuleFor(x => x.Month)
                .InclusiveBetween(1, 12).WithMessage("Tháng phải trong khoảng 1-12.");

            RuleFor(x => x.ShiftId)
                .NotEmpty().WithMessage("Ca làm việc không được để trống.");

            RuleFor(x => x.FacilityId)
                .NotEmpty().WithMessage("Cơ sở làm việc không được để trống.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Trạng thái không được để trống.")
                .MaximumLength(20).WithMessage("Trạng thái tối đa 20 ký tự.");

            RuleFor(x => x.Note)
                .MaximumLength(300).WithMessage("Ghi chú tối đa 300 ký tự.");

            RuleForEach(x => x.WorkingDaysOfWeek)
                .IsInEnum()
                .When(x => x.WorkingDaysOfWeek is { Count: > 0 });
        }
    }
}
