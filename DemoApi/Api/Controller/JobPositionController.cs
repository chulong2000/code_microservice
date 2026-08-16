using DemoApi.Domain.IServices;
using DemoApi.Domain.ModelMetas;
using DemoApi.Domain.Models;
using DemoApi.Domain.ViewModels;
using GHM.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DemoApi.Api.Controller
{

    [Produces("application/json")]
    [ApiController]
    [Route("api/v1/job-position")]
    [SwaggerTag("Quản lý danh sách vị trí công việc")]
    public class JobPositionController(IJobPositionService service) : ControllerBase
    {
       [HttpGet]
       [SwaggerOperation(
      Summary = "Danh sách các công việc",
      Description = "Trả về toàn bộ danh mục các công việc.",
      OperationId = "GetJobPosition")]
        [ProducesResponseType(typeof(ActionResultResponse<List<JobPositionViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList(Guid educationLevelId, string? keyword)
        {
            var result = await service.GetListAsync(educationLevelId, keyword);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Chi tiết về vị trí nghề nghiệp",
            Description = "Trả về `Code = -99` (HTTP 404) khi không tìm thấy.",
            OperationId = "GetJobPositionDetail")]
        [ProducesResponseType(typeof(ActionResultResponse<JobPositionViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetail(
            [SwaggerParameter("Id của trình độ học vấn", Required = true)] Guid id)
        {
            var result = await service.GetDetailAsync(id);
            return result.Code <= 0 ? NotFound(result) : Ok(result);
        }


        [HttpPost]
        [SwaggerOperation(
            Summary = "Tạo mới vị trí nghề nghiệp",
            Description = "Trả về `Code = -1` (HTTP 400) khi tên đã tồn tại.",
            OperationId = "Create Job Position")]
        [ProducesResponseType(typeof(ActionResultResponse<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody, SwaggerRequestBody("Thông tin vị trí nghề nghiệp ", Required = true)] JobPositionMeta meta)
        {
            var result = await service.CreateAsync(meta);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Cập nhật vị trí nghề nghiệp",
            Description = "Trả về `Code = -1` (HTTP 400) khi tên trùng, `Code = -99` khi không tìm thấy.",
            OperationId = "Update Job Position")]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(
            [SwaggerParameter("Id của vị trí nghề nghiệp", Required = true)] Guid id,
            [FromBody, SwaggerRequestBody("Thông tin cập nhật", Required = true)] JobPositionMeta meta)
        {
            var result = await service.UpdateAsync(id, meta);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [SwaggerOperation(
            Summary = "Xoá (mềm) vị trí công việc",
            Description = "Trả về `Code = -99` (HTTP 400) khi không tìm thấy.",
            OperationId = "Delete Job Position")]
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
