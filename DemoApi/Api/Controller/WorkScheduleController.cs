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
    [Route("api/v1/work-schedules")]
    [SwaggerTag("Quản lý Lịch làm việc")]
    public class WorkScheduleController(IWorkScheduleService service) : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(
            Summary = "Danh sách lịch làm việc",
            Description = "Trả về toàn bộ lịch làm việc chưa bị xoá (chưa hỗ trợ phân trang/tìm kiếm).",
            OperationId = "GetWorkSchedules")]
        [ProducesResponseType(typeof(ActionResultResponse<List<WorkScheduleViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList()
        {
            var result = await service.GetListAsync();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [SwaggerOperation(
            Summary = "Chi tiết một lịch làm việc",
            Description = "Trả về `Code = -99` (HTTP 404) khi không tìm thấy.",
            OperationId = "GetWorkScheduleDetail")]
        [ProducesResponseType(typeof(ActionResultResponse<WorkScheduleViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetail(
            [SwaggerParameter("Id của lịch làm việc", Required = true)] Guid id)
        {
            var result = await service.GetDetailAsync(id);
            return result.Code <= 0 ? NotFound(result) : Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Tạo lịch làm việc",
            OperationId = "CreateWorkSchedule")]
        [ProducesResponseType(typeof(ActionResultResponse<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody, SwaggerRequestBody("Thông tin lịch làm việc cần tạo", Required = true)] WorkScheduleMeta meta)
        {
            var result = await service.CreateAsync(meta);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }

        [HttpPut("{id:guid}")]
        [SwaggerOperation(
            Summary = "Cập nhật lịch làm việc",
            Description = "Trả về `Code = -99` khi không tìm thấy.",
            OperationId = "UpdateWorkSchedule")]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(
            [SwaggerParameter("Id của lịch làm việc", Required = true)] Guid id,
            [FromBody, SwaggerRequestBody("Thông tin cập nhật", Required = true)] WorkScheduleMeta meta)
        {
            var result = await service.UpdateAsync(id, meta);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [SwaggerOperation(
            Summary = "Xoá (mềm) lịch làm việc",
            Description = "Trả về `Code = -99` khi không tìm thấy.",
            OperationId = "DeleteWorkSchedule")]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(
            [SwaggerParameter("Id của lịch làm việc", Required = true)] Guid id)
        {
            var result = await service.DeleteAsync(id);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }
    }
}
