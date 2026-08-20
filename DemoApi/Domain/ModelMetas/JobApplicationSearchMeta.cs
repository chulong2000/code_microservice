namespace DemoApi.Domain.ModelMetas
{
    public class JobApplicationSearchMeta
    {
        public string? Keyword { get; set; }          // Tìm theo tên/email
        public Guid? JobPositionId { get; set; }        // Optional, lọc theo vị trí
        public DateTime? AppliedFrom { get; set; }        // Optional, lọc theo khoảng ngày nộp
        public DateTime? AppliedTo { get; set; }
    }
}
