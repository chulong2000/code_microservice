using System.ComponentModel.DataAnnotations;

namespace DemoApi.Domain.ModelMetas
{
    public class EducationLevelMeta
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
