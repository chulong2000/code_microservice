namespace DemoApi.Domain.Models
{
    public class EducationLevel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;   // max 100, unique
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
