namespace DemoApi.Domain.Models
{
    public class Shift
    {
        public Guid Id { get; set; }

        // NULL = áp dụng chung mọi cơ sở
        public Guid? FacilityId { get; set; }
        public Facility? Facility { get; set; }

        public string Name { get; set; } = string.Empty;   // bắt buộc, max 100
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
