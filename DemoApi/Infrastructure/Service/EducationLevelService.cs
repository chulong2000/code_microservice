using DemoApi.Domain.IRepository;
using DemoApi.Domain.IServices;
using DemoApi.Domain.Mapper;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;
using DemoApi.Domain.Mapper;
using System.Xml.Linq;

namespace DemoApi.Infrastructure.Service
{
    public class EducationLevelService : IEducationLevelService
    {
        private readonly IEducationLevelRepository _educationRepo;

        public EducationLevelService (IEducationLevelRepository educationRepo)
        {
            _educationRepo = educationRepo;
        }

        public async Task<ActionResultResponse<PagedResultViewModel<EducationLevelViewModel>>> GetListAsync(PagingRequestMeta request)
        {
            var (entities, totalRecords) = await _educationRepo.SelectListAsync(request);

            var data = new PagedResultViewModel<EducationLevelViewModel>
            {
                Items = entities.Select(EducationLevelMapper.MapToViewModel).ToList(),
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = totalRecords
            };

            // Constructor (T data) tự set Code = 1 — dùng cho case thành công đơn giản, không cần message riêng.
            return new ActionResultResponse<PagedResultViewModel<EducationLevelViewModel>>(data);
        }

        public async Task<ActionResultResponse<EducationLevelViewModel>> GetDetailAsync(Guid id)
        {
            var entity = await _educationRepo.SelectByIdAsync(id);
            return entity is null 
                   ? new ActionResultResponse<EducationLevelViewModel>(-99, "Không tìm thấy trình độ học vấn.")
                   : new ActionResultResponse<EducationLevelViewModel>(EducationLevelMapper.MapToViewModel(entity));
        }

        public async Task<ActionResultResponse<Guid>> CreateAsync(EducationLevelMeta meta)
        {
            var name = meta.Name.Trim();
            var entity = new EducationLevel
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = meta.Description?.Trim(),
                Order = meta.Order,
                CreatedAt = DateTime.Now,
                ParentId = meta.ParentId,
            };

            var result = await _educationRepo.InsertAsync(entity);

            return result switch
            {
                1 => new ActionResultResponse<Guid>(1, "Cập nhật thành công."),
                -1 => new ActionResultResponse<Guid>(-1, $"Trình độ học vấn \"{name}\" đã tồn tại."),
                -2 => new ActionResultResponse<Guid>(-2, "Danh mục cha không tồn tại"),
                _ => new ActionResultResponse<Guid>(-99, "Không tìm thấy trình độ học vấn.")
            };

        }

        public async Task<ActionResultResponse> UpdateAsync(Guid id, EducationLevelMeta meta)
        {
            var name = meta.Name.Trim();

            //if (await _educationRepo.ExistsNameAsync(name, id))
            //    return new ActionResultResponse(-1, $"Trình độ học vấn \"{name}\" đã tồn tại.");

            var entity = new EducationLevel
            {
                Id = id,
                Name = name,
                Description = meta.Description?.Trim(),
                Order = meta.Order,
                ParentId = meta.ParentId,
                UpdatedAt = DateTime.Now
            };

            var result = await _educationRepo.UpdateAsync(entity);
            return result switch
            {
                1 => new ActionResultResponse(1, "Cập nhật thành công."),
                -1 => new ActionResultResponse(-1, $"Trình độ học vấn \"{name}\" đã tồn tại."),
                -2 => new ActionResultResponse(-2, "Danh mục cha không tồn tại."),
                -3 => new ActionResultResponse(-3, "Không thể chọn chính nó làm danh mục cha."),
                -4 => new ActionResultResponse(-4, "Không thể chọn danh mục con/cháu làm danh mục cha vì sẽ gây vòng lặp."),
                _ => new ActionResultResponse(-99, "Không tìm thấy trình độ học vấn.")
            };
        }

        public async Task<ActionResultResponse> DeleteAsync(Guid id)
        {
            var result = await _educationRepo.SoftDeleteAsync(id);
            return result switch
            {
                1 => new ActionResultResponse(1, "Xóa thành công (bao gồm toàn bộ danh mục con nếu có)."),
                -1 => new ActionResultResponse(-1, "Không thể xóa vì danh mục này hoặc danh mục con của nó đang có vị trí công việc tham chiếu."),
                _ => new ActionResultResponse(-99, "Không tìm thấy trình độ học vấn.")
            };
        }

        public async Task<ActionResultResponse<List<JobPositionViewModel>>> GetListJobPositionByEducationLevelId(Guid id)
        {
            var entities = await _educationRepo.GetListJobPositionByEducationLevelId(id);
            var data = entities.Select(JobPositionMapper.MapToViewModel).ToList();

            return new ActionResultResponse<List<JobPositionViewModel>>(data);

        }

        public async Task<ActionResultResponse<List<EducationLevelViewModel>>> GetTreeAsync()
        {
            var flat = await _educationRepo.GetAllEducationLevelTree();
            var tree = BuildTree(flat, null);

            return new ActionResultResponse<List<EducationLevelViewModel>>(tree);
        }

        // Đệ quy: với mỗi node có Id = parentId, tìm tất cả node con (ParentId = Id đó), rồi lại tự tìm con của con.
        private static List<EducationLevelViewModel> BuildTree(List<EducationLevel> flat, Guid? parentId)
        {
            return flat
                .Where(x => x.ParentId == parentId)
                .OrderBy(x => x.Order)
                .Select(x =>
                {
                    var node = EducationLevelMapper.MapToViewModel(x);
                    node.Children = BuildTree(flat, x.Id);
                    return node;
                })
                .ToList();
        }

    }
}
