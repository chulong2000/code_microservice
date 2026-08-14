namespace DemoApi.Domain.Models
{
    public class EducationLevelSalaryCoefficient
    {
        public Guid Id { get; set; }

        // FK + unique -> đảm bảo 1-1
        public Guid EducationLevelId { get; set; }

        public decimal BaseCoefficient { get; set; }        // hệ số lương cơ bản, VD: 2.34
        public decimal? AllowancePercentage { get; set; }    // % phụ cấp thêm nếu có
        public DateTime EffectiveFrom { get; set; }           // ngày áp dụng hệ số
        public string? Notes { get; set; }                    // max 500

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
