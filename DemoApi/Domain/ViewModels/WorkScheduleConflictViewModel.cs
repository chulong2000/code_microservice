namespace DemoApi.Domain.ViewModels
{
    public class WorkScheduleConflictViewModel
    {
        public bool HasConflict { get; set; }

        public Guid? LeaveRequestId { get; set; }
        public string? LeaveType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Status { get; set; }
    }
}
