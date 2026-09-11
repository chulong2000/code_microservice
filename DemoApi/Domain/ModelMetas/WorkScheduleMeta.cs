namespace DemoApi.Domain.ModelMetas
{
    public class WorkScheduleMeta
    {
        public Guid EmployeeId { get; set; }
        public Guid ShiftId { get; set; }
        public Guid FacilityId { get; set; }

        public DateTime WorkDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Note { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
