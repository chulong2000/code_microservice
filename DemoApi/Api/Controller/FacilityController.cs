using DemoApi.Domain.IServices;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DemoApi.Api.Controller
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/v1/facilities")]
    [SwaggerTag("Quản lý danh mục Cơ sở")]
    public class FacilityController(IFacilityService service) : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(
            Summary = "Danh sách cơ sở",
            Description = "Trả về danh mục cơ sở chưa bị xoá, hỗ trợ phân trang, sắp xếp và tìm kiếm theo từ khoá (Name, Address). " +
                          "Mặc định sắp xếp theo Name tăng dần.",
            OperationId = "GetFacilities")]
        [ProducesResponseType(typeof(ActionResultResponse<PagedResultViewModel<FacilityViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList(
            [FromQuery, SwaggerParameter("Tham số phân trang (pageIndex, pageSize), sắp xếp (sortColumn, sortDescending) và tìm kiếm (keyword)")] PagingRequestMeta request)
        {
            var result = await service.GetListAsync(request);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [SwaggerOperation(
            Summary = "Chi tiết một cơ sở",
            Description = "Trả về `Code = -99` (HTTP 404) khi không tìm thấy.",
            OperationId = "GetFacilityDetail")]
        [ProducesResponseType(typeof(ActionResultResponse<FacilityViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetail(
            [SwaggerParameter("Id của cơ sở", Required = true)] Guid id)
        {
            var result = await service.GetDetailAsync(id);
            return result.Code <= 0 ? NotFound(result) : Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Tạo cơ sở",
            Description = "Trả về `Code = -1` (HTTP 400) khi tên đã tồn tại.",
            OperationId = "CreateFacility")]
        [ProducesResponseType(typeof(ActionResultResponse<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody, SwaggerRequestBody("Thông tin cơ sở cần tạo", Required = true)] FacilityMeta meta)
        {
            var result = await service.CreateAsync(meta);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }

        [HttpPut("{id:guid}")]
        [SwaggerOperation(
            Summary = "Cập nhật cơ sở",
            Description = "Trả về `Code = -1` (HTTP 400) khi tên trùng, `Code = -99` khi không tìm thấy.",
            OperationId = "UpdateFacility")]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(
            [SwaggerParameter("Id của cơ sở", Required = true)] Guid id,
            [FromBody, SwaggerRequestBody("Thông tin cập nhật", Required = true)] FacilityMeta meta)
        {
            var result = await service.UpdateAsync(id, meta);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [SwaggerOperation(
            Summary = "Xoá (mềm) cơ sở",
            Description = "Trả về `Code = -99` (HTTP 400) khi không tìm thấy.",
            OperationId = "DeleteFacility")]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(
            [SwaggerParameter("Id của cơ sở", Required = true)] Guid id)
        {
            var result = await service.DeleteAsync(id);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }
    }
}
