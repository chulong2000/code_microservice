namespace DemoApi.Domain.Models
{
    public class LeaveRequest
    {
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; } = new Employee();

        public string LeaveType { get; set; } = string.Empty;   // Annual / Sick / Unpaid / Other, max 30
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? Reason { get; set; }                       // max 300
        public string Status { get; set; } = string.Empty;       // Pending / Approved / Rejected, max 20
        public Guid? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
