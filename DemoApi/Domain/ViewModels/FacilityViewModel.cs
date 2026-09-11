namespace DemoApi.Domain.ViewModels
{
    public class FacilityViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
