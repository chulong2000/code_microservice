namespace DemoApi.Domain.ModelMetas
{
    public class WorkScheduleEntryRequest
    {
        public Guid EmployeeId { get; set; }

        public DateTime WorkDate { get; set; }

        public Guid ShifId { get; set; }

        public string? Note { get; set; }

        public Guid CreatBy { get; set; }
    }
}