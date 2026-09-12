namespace DemoApi.Domain.ModelMetas
{
    public class WorkScheduleBulkCreateMeta
    {
        public List<Guid> EmployeeIds { get; set; } = new();

        public int Year { get; set; }
        public int Month { get; set; }   // 1 - 12

        public Guid ShiftId { get; set; }
        public Guid FacilityId { get; set; }

        // Null hoặc rỗng = áp dụng tất cả các ngày trong tháng. VD: chỉ Thứ 2 - Thứ 6.
        public List<DayOfWeek>? WorkingDaysOfWeek { get; set; }

        public string Status { get; set; } = string.Empty;
        public string? Note { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
