using DemoApi.Domain.ModelMetas;
using FluentValidation;

namespace DemoApi.Infrastructure.Validation
{
    public class JobApplicationMetaValidator : AbstractValidator<JobApplicationMeta>
    {
        public JobApplicationMetaValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Tên của bạn không được để trống")
                .MaximumLength(60).WithMessage("Tên tối đa 60 ký tự");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống")
                .MaximumLength(150).WithMessage("Mô tả tối đa 150");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("PhoneNumber không được để trống")
                .MaximumLength(150).WithMessage("Mô tả tối đa 150");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender không được để trống")
                .MaximumLength(150).WithMessage("Mô tả tối đa 150");

            RuleFor(x => x.CvFileUrl)
                .NotEmpty().WithMessage("CvFileUrl không được để trống")
                .MaximumLength(150).WithMessage("Mô tả tối đa 150");

            RuleFor(x => x.CoverLetter)
                .NotEmpty().WithMessage("CoverLetter không được để trống")
                .MaximumLength(150).WithMessage("Mô tả tối đa 150");

            RuleFor(x => x.YearsOfExperience)
                .ExclusiveBetween(1, 30).WithMessage("Số năm kinh nghiệm nằm trong khoảng 1 - 30 năm");

        }
    }
}
