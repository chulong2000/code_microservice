using DemoApi.Domain.ModelMetas;
using FluentValidation;

namespace DemoApi.Infrastructure.Validation
{
    public class WorkScheduleBulkCreateMetaValidator : AbstractValidator<WorkScheduleBulkCreateMeta>
    {
        public WorkScheduleBulkCreateMetaValidator()
        {
            RuleFor(x => x.FacilityID)
                .NotNull().WithMessage("Phải có có thông tin về địa chỉ làm việc");

            RuleFor(x => x.Entries)
                .NotEmpty().WithMessage("Danh sách lịch làm việc không được để trống.");

            RuleForEach(x => x.Entries)
                .SetValidator(new WorkScheduleEntryRequestValidator())
                .When(x => x.Entries is { Count: > 0 });
        }
    }

    public class WorkScheduleEntryRequestValidator : AbstractValidator<WorkScheduleEntryRequest>
    {
        public WorkScheduleEntryRequestValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("Id nhân viên không hợp lệ.");

            RuleFor(x => x.WorkDate)
                .NotEmpty().WithMessage("Ngày làm việc không được để trống.");

            RuleFor(x => x.ShifId)
                .NotEmpty().WithMessage("Ca làm việc không được để trống.");

            RuleFor(x => x.CreatBy)
                .NotEmpty().WithMessage("Người tạo không được để trống.");

            RuleFor(x => x.Note)
                .MaximumLength(300).WithMessage("Ghi chú tối đa 300 ký tự.");
        }
    }
}
