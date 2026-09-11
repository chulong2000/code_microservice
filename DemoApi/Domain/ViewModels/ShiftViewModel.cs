namespace DemoApi.Domain.ViewModels
{
    public class ShiftViewModel
    {
        public Guid Id { get; set; }
        public Guid? FacilityId { get; set; }
        public string Name { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
