namespace DemoApi.Domain.ModelMetas
{
    public class ShiftMeta
    {
        public Guid? FacilityId { get; set; }
        public string Name { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
