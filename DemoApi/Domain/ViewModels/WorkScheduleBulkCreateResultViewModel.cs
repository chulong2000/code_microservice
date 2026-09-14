namespace DemoApi.Domain.ViewModels
{
    public class WorkScheduleBulkCreateResultViewModel
    {
        public int TotalRequested { get; set; }
        public int CreatedCount { get; set; }

        // Số entry bị bỏ qua vì đã có lịch làm việc trùng (EmployeeId + WorkDate) từ trước.
        public int DuplicateScheduleSkippedCount { get; set; }

        // Các entry bị bỏ qua vì nhân viên đã có đơn nghỉ phép (Pending/Approved) trùng ngày.
        public List<WorkScheduleLeaveConflictItem> LeaveConflictSkipped { get; set; } = new();
    }

    public class WorkScheduleLeaveConflictItem
    {
        public Guid EmployeeId { get; set; }
        public DateTime WorkDate { get; set; }
        public string LeaveStatus { get; set; } = string.Empty;
    }
}
