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
    [Route("api/v1/leave-requests")]
    [SwaggerTag("Quản lý Đơn nghỉ phép")]
    public class LeaveRequestController(ILeaveRequestService service) : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(
            Summary = "Danh sách đơn nghỉ phép",
            Description = "Trả về đơn nghỉ phép chưa bị xoá, có thể lọc theo `employeeId` và/hoặc `status` (chưa hỗ trợ phân trang).",
            OperationId = "GetLeaveRequests")]
        [ProducesResponseType(typeof(ActionResultResponse<List<LeaveRequestViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList(
            [FromQuery, SwaggerParameter("Lọc theo Id nhân viên")] Guid? employeeId,
            [FromQuery, SwaggerParameter("Lọc theo trạng thái: Pending / Approved / Rejected")] string? status)
        {
            var result = await service.GetListAsync(employeeId, status);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [SwaggerOperation(
            Summary = "Chi tiết một đơn nghỉ phép",
            Description = "Trả về `Code = -99` (HTTP 404) khi không tìm thấy.",
            OperationId = "GetLeaveRequestDetail")]
        [ProducesResponseType(typeof(ActionResultResponse<LeaveRequestViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetail(
            [SwaggerParameter("Id của đơn nghỉ phép", Required = true)] Guid id)
        {
            var result = await service.GetDetailAsync(id);
            return result.Code <= 0 ? NotFound(result) : Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Tạo đơn nghỉ phép",
            OperationId = "CreateLeaveRequest")]
        [ProducesResponseType(typeof(ActionResultResponse<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody, SwaggerRequestBody("Thông tin đơn nghỉ phép cần tạo", Required = true)] LeaveRequestMeta meta)
        {
            var result = await service.CreateAsync(meta);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }

        [HttpPut("{id:guid}")]
        [SwaggerOperation(
            Summary = "Cập nhật đơn nghỉ phép",
            Description = "Trả về `Code = -99` khi không tìm thấy.",
            OperationId = "UpdateLeaveRequest")]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(
            [SwaggerParameter("Id của đơn nghỉ phép", Required = true)] Guid id,
            [FromBody, SwaggerRequestBody("Thông tin cập nhật", Required = true)] LeaveRequestMeta meta)
        {
            var result = await service.UpdateAsync(id, meta);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [SwaggerOperation(
            Summary = "Xoá (mềm) đơn nghỉ phép",
            Description = "Trả về `Code = -99` khi không tìm thấy.",
            OperationId = "DeleteLeaveRequest")]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(
            [SwaggerParameter("Id của đơn nghỉ phép", Required = true)] Guid id)
        {
            var result = await service.DeleteAsync(id);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }

        [HttpPut("{id:guid}/approve")]
        [SwaggerOperation(
            Summary = "Duyệt đơn nghỉ phép",
            Description = "Chỉ duyệt được đơn đang ở trạng thái `Pending`. Trả về `Code = -99` khi không tìm thấy hoặc đơn đã được xử lý.",
            OperationId = "ApproveLeaveRequest")]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Approve(
            [SwaggerParameter("Id của đơn nghỉ phép", Required = true)] Guid id,
            [FromBody, SwaggerRequestBody("Người duyệt (tuỳ chọn)")] LeaveRequestApproveMeta? meta)
        {
            var result = await service.ApproveAsync(id, meta ?? new LeaveRequestApproveMeta());
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }

        [HttpPut("{id:guid}/reject")]
        [SwaggerOperation(
            Summary = "Từ chối đơn nghỉ phép",
            Description = "Chỉ từ chối được đơn đang ở trạng thái `Pending`. Trả về `Code = -99` khi không tìm thấy hoặc đơn đã được xử lý.",
            OperationId = "RejectLeaveRequest")]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Reject(
            [SwaggerParameter("Id của đơn nghỉ phép", Required = true)] Guid id,
            [FromBody, SwaggerRequestBody("Người từ chối (tuỳ chọn)")] LeaveRequestRejectMeta? meta)
        {
            var result = await service.RejectAsync(id, meta ?? new LeaveRequestRejectMeta());
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }
    }
}
