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
    [Route("api/v1/employees")]
    [SwaggerTag("Quản lý danh mục Nhân viên")]
    public class EmployeeController(IEmployeeService service) : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(
            Summary = "Danh sách nhân viên",
            Description = "Trả về toàn bộ nhân viên chưa bị xoá (chưa hỗ trợ phân trang/tìm kiếm).",
            OperationId = "GetEmployees")]
        [ProducesResponseType(typeof(ActionResultResponse<List<EmployeeViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] Guid? facilityId, [FromQuery] Guid? jobPositionId, [FromQuery] string? status)
        {
            var result = await service.GetListAsync(facilityId, jobPositionId, status);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [SwaggerOperation(
            Summary = "Chi tiết một nhân viên",
            Description = "Trả về `Code = -99` (HTTP 404) khi không tìm thấy.",
            OperationId = "GetEmployeeDetail")]
        [ProducesResponseType(typeof(ActionResultResponse<EmployeeViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetail(
            [SwaggerParameter("Id của nhân viên", Required = true)] Guid id)
        {
            var result = await service.GetDetailAsync(id);
            return result.Code <= 0 ? NotFound(result) : Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Tạo nhân viên",
            OperationId = "CreateEmployee")]
        [ProducesResponseType(typeof(ActionResultResponse<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody, SwaggerRequestBody("Thông tin nhân viên cần tạo", Required = true)] EmployeeMeta meta)
        {
            var result = await service.CreateAsync(meta);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }

        [HttpPut("{id:guid}")]
        [SwaggerOperation(
            Summary = "Cập nhật nhân viên",
            Description = "Trả về `Code = -99` khi không tìm thấy.",
            OperationId = "UpdateEmployee")]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(
            [SwaggerParameter("Id của nhân viên", Required = true)] Guid id,
            [FromBody, SwaggerRequestBody("Thông tin cập nhật", Required = true)] EmployeeMeta meta)
        {
            var result = await service.UpdateAsync(id, meta);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [SwaggerOperation(
            Summary = "Xoá (mềm) nhân viên",
            Description = "Trả về `Code = -99` khi không tìm thấy.",
            OperationId = "DeleteEmployee")]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(
            [SwaggerParameter("Id của nhân viên", Required = true)] Guid id)
        {
            var result = await service.DeleteAsync(id);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }
    }
}
