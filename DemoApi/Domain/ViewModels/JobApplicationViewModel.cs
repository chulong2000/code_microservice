using DemoApi.Domain.Models;

namespace DemoApi.Domain.ViewModels
{
    public class JobApplicationViewModel
    {
        public Guid Id { get; set; }

        public JobPosition JobPosition { get; set; } = null!;

        // Thông tin ứng viên
        public string FullName { get; set; } = string.Empty;      // bắt buộc, max 150
        public string Email { get; set; } = string.Empty;          // bắt buộc, max 150
        public string PhoneNumber { get; set; } = string.Empty;    // bắt buộc, max 20
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }                        // max 20

        // Hồ sơ CV
        public string CvFileUrl { get; set; } = string.Empty;       // đường dẫn file CV đã upload, max 500
        public string? CoverLetter { get; set; }                     // thư xin việc, max 2000
        public int? YearsOfExperience { get; set; }                  // số năm kinh nghiệm

        public DateTime AppliedAt { get; set; }                        // ngày nộp hồ sơ
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
