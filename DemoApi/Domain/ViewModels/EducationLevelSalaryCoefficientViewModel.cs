using DemoApi.Domain.Models;

namespace DemoApi.Domain.ViewModels
{
    public class EducationLevelSalaryCoefficientViewModel
    {

        public Guid Id { get; set; }
        public EducationLevel educationLevel { get; set; }

        public decimal BaseCoefficient { get; set; }        // hệ số lương cơ bản, VD: 2.34
        public decimal? AllowancePercentage { get; set; }    // % phụ cấp thêm nếu có
        public DateTime EffectiveFrom { get; set; }           // ngày áp dụng hệ số
        public string? Notes { get; set; }                    // max 500

    }
}
