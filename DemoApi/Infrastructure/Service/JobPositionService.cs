using DemoApi.Domain.IRepository;
using DemoApi.Domain.IServices;
using DemoApi.Domain.Mapper;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace DemoApi.Infrastructure.Service
{
    public class JobPositionService : IJobPositionService
    {
        private readonly IJobPositionRepository _jobPositionRepo;

        // .NET cần "tiêm" IJobPositionService vào đây
        public JobPositionService(IJobPositionRepository jobPositionRepo)
        {
            _jobPositionRepo = jobPositionRepo;
        }
        public async Task<ActionResultResponse<Guid>> CreateAsync(JobPositionMeta meta)
        {
            var name = meta.Title.Trim();

            if (await _jobPositionRepo.ExistsNameAsync(name, null))
                return new ActionResultResponse<Guid>(-1, $"Vị trí công việc \"{name}\" đã tồn tại.");

            var entity = new JobPosition
            {
                Id = Guid.NewGuid(),
                Title = name,
                Department = meta.Department,
                OpenSlots = meta.OpenSlots,
                MinimumEducationLevelId = meta.MinimumEducationLevelId,
                ParentId = meta.ParentId,
                IsOpen = meta.IsOpen,
                CreatedAt = DateTime.Now
            };

            var result = await _jobPositionRepo.InsertAsync(entity);
            return result switch
            {
                1 => new ActionResultResponse<Guid>(1, "Cập nhật thành công."),
                -1 => new ActionResultResponse<Guid>(-1, $"Vị trí công việc \"{name}\" đã tồn tại."),
                -2 => new ActionResultResponse<Guid>(-2, "Danh mục cha không tồn tại"),
                _ => new ActionResultResponse<Guid>(-99, "Không tìm thấy vị trí công việc.")
            };
        }

        public async Task<ActionResultResponse> DeleteAsync(Guid id)
        {
            var result = await _jobPositionRepo.SoftDeleteAsync(id);
            return result switch
            {
                1 => new ActionResultResponse(1, "Xóa thành công (bao gồm toàn bộ danh mục con nếu có)."),
                -1 => new ActionResultResponse(-1, "Không thể xóa vì danh mục này hoặc danh mục con của nó đang có vị trí công việc tham chiếu."),
                _ => new ActionResultResponse(-99, "Không tìm thấy vị trí công việc.")
            };
        }

        public async Task<ActionResultResponse<JobPositionViewModel>> GetDetailAsync(Guid id)
        {
            var entity = await _jobPositionRepo.SelectByIdAsync(id);

            return entity is null 
                ? new ActionResultResponse<JobPositionViewModel>(-99, "Không tìm thấy vị trí công việc.")
                : new ActionResultResponse<JobPositionViewModel>(JobPositionMapper.MapToViewModel(entity));
        }

        public async Task<ActionResultResponse<List<JobPositionViewModel>>> GetListAsync(Guid? educationLevelId, string keyword)
        {
            var entities = await _jobPositionRepo.SelectListAsync(educationLevelId, keyword);
            var data = entities.Select(JobPositionMapper.MapToViewModel).ToList();

            // Constructor (T data) tự set Code = 1 — dùng cho case thành công đơn giản, không cần message riêng.
            return new ActionResultResponse<List<JobPositionViewModel>>(data);
        }

        public async Task<ActionResultResponse<List<JobPositionViewModel>>> GetTreeAsync()
        {
            var flat = await _jobPositionRepo.GetAllJobPositionTree();
            var tree = BuildTree(flat, null);

            return new ActionResultResponse<List<JobPositionViewModel>>(tree);
        }

        // Đệ quy: với mỗi node có Id = parentId, tìm tất cả node con (ParentId = Id đó), rồi lại tự tìm con của con.
        private static List<JobPositionViewModel> BuildTree(List<JobPosition> flat, Guid? parentId)
        {
            return flat
                .Where(x => x.ParentId == parentId)
                .Select(x =>
                {
                    var node = JobPositionMapper.MapToViewModel(x);
                    node.Children = BuildTree(flat, x.Id);
                    return node;
                })
                .ToList();
        }

        public async Task<ActionResultResponse> UpdateAsync(Guid id, JobPositionMeta meta)
        {
            var name = meta.Title.Trim();

            if (await _jobPositionRepo.ExistsNameAsync(name, id))
                return new ActionResultResponse(-1, $"Trình độ học vấn \"{name}\" đã tồn tại.");

            var entity = new JobPosition
            {
                Id = id,
                Title = name,
                OpenSlots = meta.OpenSlots,
                Department = meta.Department,
                MinimumEducationLevelId = meta.MinimumEducationLevelId,
                ParentId = meta.ParentId,
                IsOpen = meta.IsOpen,
                UpdatedAt = DateTime.Now,
            };

            var result = await _jobPositionRepo.UpdateAsync(entity);
            return result switch
            {
                1 => new ActionResultResponse(1, "Cập nhật thành công."),
                -1 => new ActionResultResponse(-1, $"Vị trí công việc \"{name}\" đã tồn tại."),
                -2 => new ActionResultResponse(-2, "Danh mục cha không tồn tại."),
                -3 => new ActionResultResponse(-3, "Không thể chọn chính nó làm danh mục cha."),
                -4 => new ActionResultResponse(-4, "Không thể chọn danh mục con/cháu làm danh mục cha vì sẽ gây vòng lặp."),
                _ => new ActionResultResponse(-99, "Không tìm thấy vị trí công việc.")
            };
        }

        

        
    }
}
