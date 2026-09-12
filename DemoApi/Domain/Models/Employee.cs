namespace DemoApi.Domain.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;   // bắt buộc, unique, max 50

        // Liên kết ngược tới hồ sơ ứng tuyển (nếu có)
        public Guid? JobApplicationId { get; set; }
        public JobApplication JobApplication { get; set; } = new JobApplication();

        // Tái sử dụng bảng JobPosition làm chức danh
        public Guid JobPositionId { get; set; }
        public JobPosition JobPosition { get; set; } = new JobPosition();

        // Cơ sở làm việc chính
        public Guid PrimaryFacilityId { get; set; }
        public Facility PrimaryFacility { get; set; } = new Facility();

        public string FullName { get; set; } = string.Empty;   // bắt buộc, max 200
        public string? Email { get; set; }                       // max 150
        public string? PhoneNumber { get; set; }                 // max 20
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }                       // max 20

        public DateTime HireDate { get; set; }
        public string Status { get; set; } = string.Empty;      // Active / OnLeave / Terminated, max 20

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
