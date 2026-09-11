namespace DemoApi.Domain.Models
{
    public class WorkSchedule
    {
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; } = new Employee();

        public Guid ShiftId { get; set; }
        public Shift Shift { get; set; } = new Shift();

        // Cơ sở làm ca đó (có thể khác PrimaryFacilityId của nhân viên)
        public Guid FacilityId { get; set; }
        public Facility Facility { get; set; } = new Facility();

        public DateTime WorkDate { get; set; }
        public string Status { get; set; } = string.Empty;   // Draft / Published / Confirmed / Cancelled, max 20
        public string? Note { get; set; }                      // max 300
        public Guid? CreatedBy { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
