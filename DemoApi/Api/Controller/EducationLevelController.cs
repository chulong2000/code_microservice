using DemoApi.Domain.IRepository;
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
    [Route("api/v1/education-levels")]
    [SwaggerTag("Quản lý danh mục Trình độ học vấn")]
    public class EducationLevelController(IEducationLevelService service) : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(
       Summary = "Danh sách trình độ học vấn",
       Description = "Trả về toàn bộ danh mục trình độ học vấn chưa bị xoá, sắp xếp theo Order.",
       OperationId = "GetEducationLevels")]
        [ProducesResponseType(typeof(ActionResultResponse<List<EducationLevelViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList()
        {
            var result = await service.GetListAsync();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [SwaggerOperation(
            Summary = "Chi tiết một trình độ học vấn",
            Description = "Trả về `Code = -99` (HTTP 404) khi không tìm thấy.",
            OperationId = "GetEducationLevelDetail")]
        [ProducesResponseType(typeof(ActionResultResponse<EducationLevelViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetail(
            [SwaggerParameter("Id của trình độ học vấn", Required = true)] Guid id)
        {
            var result = await service.GetDetailAsync(id);
            return result.Code <= 0 ? NotFound(result) : Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Tạo trình độ học vấn",
            Description = "Trả về `Code = -1` (HTTP 400) khi tên đã tồn tại.",
            OperationId = "CreateEducationLevel")]
        [ProducesResponseType(typeof(ActionResultResponse<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody, SwaggerRequestBody("Thông tin trình độ học vấn cần tạo", Required = true)] EducationLevelMeta meta)
        {
            var result = await service.CreateAsync(meta);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }

        [HttpPut("{id:guid}")]
        [SwaggerOperation(
            Summary = "Cập nhật trình độ học vấn",
            Description = "Trả về `Code = -1` (HTTP 400) khi tên trùng, `Code = -99` khi không tìm thấy.",
            OperationId = "UpdateEducationLevel")]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(
            [SwaggerParameter("Id của trình độ học vấn", Required = true)] Guid id,
            [FromBody, SwaggerRequestBody("Thông tin cập nhật", Required = true)] EducationLevelMeta meta)
        {
            var result = await service.UpdateAsync(id, meta);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [SwaggerOperation(
            Summary = "Xoá (mềm) trình độ học vấn",
            Description = "Trả về `Code = -99` (HTTP 400) khi không tìm thấy.",
            OperationId = "DeleteEducationLevel")]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(
            [SwaggerParameter("Id của trình độ học vấn", Required = true)] Guid id)
        {
            var result = await service.DeleteAsync(id);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }
    }
}
