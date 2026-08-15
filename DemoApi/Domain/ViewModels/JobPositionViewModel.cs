using DemoApi.Domain.Models;

namespace DemoApi.Domain.ViewModels
{
    public class JobPositionViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;   // tên vị trí, max 150
        public string? Department { get; set; }              // khoa/phòng tuyển
        public int? OpenSlots { get; set; }                    // số lượng cần tuyển

        // FK — nhiều JobPosition cùng yêu cầu 1 mức EducationLevel tối thiểu
        public Guid MinimumEducationLevelId { get; set; }

        public string MinimumEducationLevelName { get; set; }
        public EducationLevel MinimumEducationLevel { get; set; } = null!;

        public bool IsOpen { get; set; }
        
    }

}
