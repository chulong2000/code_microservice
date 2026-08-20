using DemoApi.Domain.Models;

namespace DemoApi.Domain.ViewModels
{
    public class EducationLevelViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public EducationLevelSalaryCoefficient educationLevelSalaryCoefficient { get; set; } = null!;

        public List<JobPosition> jobPositions { get; set; }
    }
}
