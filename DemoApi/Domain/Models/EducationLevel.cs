namespace DemoApi.Domain.Models
{
    public class EducationLevel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;   // bắt buộc, unique, max 100
        public string? Description { get; set; }             // max 500
        public int Order { get; set; }                       // thứ tự hiển thị (VD: Tiểu học=1, THCS=2...)
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public EducationLevelSalaryCoefficient educationLevelSalaryCoefficient { get; set; }

        public List<JobPosition> jobPositions { get; } = [];

    }
}
