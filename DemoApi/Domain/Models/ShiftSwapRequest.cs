namespace DemoApi.Domain.Models
{
    public class ShiftSwapRequest
    {
        public Guid Id { get; set; }

        // Ca muốn đổi
        public Guid WorkScheduleId { get; set; }
        public WorkSchedule WorkSchedule { get; set; } = new WorkSchedule();

        public Guid RequestedByEmployeeId { get; set; }
        public Employee RequestedByEmployee { get; set; } = new Employee();

        public Guid? SwapWithEmployeeId { get; set; }
        public Employee? SwapWithEmployee { get; set; }

        public string? Reason { get; set; }                 // max 300
        public string Status { get; set; } = string.Empty; // Pending / Approved / Rejected, max 20

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
