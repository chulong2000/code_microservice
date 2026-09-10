namespace DemoApi.Domain.Models
{
    public class Facility
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;   // bắt buộc, max 150
        public string? Address { get; set; }                 // max 300

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
