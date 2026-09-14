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
            Description = "Trả về lịch làm việc chưa bị xoá, có thể lọc theo `employeeId`, `facilityId`, `shiftId` và/hoặc khoảng ngày `fromDate`..`toDate` " +
                          "(chưa hỗ trợ phân trang). Bỏ trống tham số nào thì không lọc theo tham số đó.",
            OperationId = "GetWorkSchedules")]
        [ProducesResponseType(typeof(ActionResultResponse<List<WorkScheduleViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList(
            [FromQuery, SwaggerParameter("Lọc theo Id nhân viên")] Guid? employeeId,
            [FromQuery, SwaggerParameter("Lọc theo Id cơ sở")] Guid? facilityId,
            [FromQuery, SwaggerParameter("Lọc từ ngày (bao gồm)")] DateTime? fromDate,
            [FromQuery, SwaggerParameter("Lọc đến ngày (bao gồm)")] DateTime? toDate,
            [FromQuery, SwaggerParameter("Lọc theo Id ca làm việc")] Guid? shiftId)
        {
            var result = await service.GetListAsync(employeeId, facilityId, fromDate, toDate, shiftId);
            return Ok(result);
        }

        [HttpGet("check-conflict")]
        [SwaggerOperation(
            Summary = "Kiểm tra xung đột với đơn nghỉ phép",
            Description = "Kiểm tra `employeeId` đã có đơn nghỉ phép (trạng thái `Pending` hoặc `Approved`) bao trùm `workDate` hay chưa, " +
                          "dùng trước khi tạo mới lịch làm việc. `shiftId` được nhận để khớp hợp đồng với API tạo lịch làm việc, " +
                          "hiện chưa dùng để lọc vì bảng LeaveRequest không lưu thông tin theo ca.",
            OperationId = "CheckWorkScheduleLeaveConflict")]
        [ProducesResponseType(typeof(ActionResultResponse<WorkScheduleConflictViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckConflict(
            [FromQuery, SwaggerParameter("Id nhân viên", Required = true)] Guid employeeId,
            [FromQuery, SwaggerParameter("Ngày làm việc cần kiểm tra", Required = true)] DateTime workDate)
        {
            var result = await service.CheckLeaveConflictAsync(employeeId, workDate);
            return Ok(result);
        }

        [HttpGet("employee/{id:guid}/upcoming")]
        [SwaggerOperation(
            Summary = "Ca làm việc sắp tới của 1 nhân viên",
            Description = "Trả về các ca đã `Published` hoặc `Confirmed` của nhân viên, có `WorkDate` từ hôm nay trở đi, sắp xếp theo ngày gần nhất trước. " +
                          "Ca `Draft` (chưa công bố) và `Cancelled`/`Rejected` không được tính là 'sắp tới'.",
            OperationId = "GetUpcomingWorkSchedulesByEmployee")]
        [ProducesResponseType(typeof(ActionResultResponse<List<WorkScheduleViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUpcomingByEmployee(
            [SwaggerParameter("Id của nhân viên", Required = true)] Guid id)
        {
            var result = await service.GetUpcomingByEmployeeAsync(id);
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

        [HttpPost("bulk-monthly")]
        [SwaggerOperation(
            Summary = "Tạo lịch làm việc hàng loạt theo tháng",
            Description = "Tạo lịch làm việc cho nhiều nhân viên cùng lúc trong một tháng (cùng ca, cùng cơ sở). " +
                          "Có thể giới hạn theo các thứ trong tuần áp dụng (VD: chỉ Thứ 2 - Thứ 6); để trống nghĩa là áp dụng tất cả các ngày trong tháng. " +
                          "Cặp (nhân viên, ngày) đã có lịch làm việc sẽ được bỏ qua, không tạo trùng. " +
                          "Cặp (nhân viên, ngày) trùng với đơn nghỉ phép (Pending/Approved) của nhân viên đó cũng sẽ được bỏ qua, " +
                          "không chặn cả batch — chi tiết các entry bị bỏ qua trả về trong `data.leaveConflictSkipped`.",
            OperationId = "BulkCreateWorkScheduleMonthly")]
        [ProducesResponseType(typeof(ActionResultResponse<WorkScheduleBulkCreateResultViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkCreateMonthly(
            [FromBody, SwaggerRequestBody("Danh sách nhân viên, tháng/năm và ca làm việc cần tạo hàng loạt", Required = true)] WorkScheduleBulkCreateMeta meta)
        {
            var result = await service.BulkCreateMonthlyAsync(meta);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }

        [HttpPut("bulk-sync")]
        [SwaggerOperation(
            Summary = "Đồng bộ lịch làm việc (Draft) theo lưới",
            Description = "Nhận toàn bộ danh sách các ô đang có trên lưới (sau khi quản lý sửa xong) trong phạm vi `facilityId` + `fromDate`..`toDate`. " +
                          "Backend tự so sánh với các dòng đang ở trạng thái `Draft` hiện có trong DB (cùng phạm vi) để tính: " +
                          "dòng bị bỏ khỏi danh sách gửi lên sẽ bị xoá (mềm), dòng mới xuất hiện sẽ được thêm mới, dòng có ở cả hai bên giữ nguyên. " +
                          "Chỉ tác động tới các dòng đang `Draft`; các dòng đã `Published`/`Confirmed` không bị ảnh hưởng. " +
                          "Gửi `entries` rỗng nghĩa là xoá toàn bộ Draft đang có trong phạm vi.",
            OperationId = "BulkSyncWorkSchedule")]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkSync(
            [FromQuery, SwaggerParameter("Id cơ sở", Required = true)] Guid facilityId,
            [FromQuery, SwaggerParameter("Từ ngày", Required = true)] DateTime fromDate,
            [FromQuery, SwaggerParameter("Đến ngày", Required = true)] DateTime toDate,
            [FromBody, SwaggerRequestBody("Toàn bộ danh sách ô lịch làm việc hiện có trên lưới", Required = true)] WorkScheduleBulkSyncMeta meta)
        {
            var result = await service.BulkSyncAsync(facilityId, fromDate, toDate, meta);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }

        [HttpPut("publish-batch")]
        [SwaggerOperation(
            Summary = "Công bố hàng loạt lịch làm việc",
            Description = "Công bố (chuyển từ trạng thái `Draft` sang `Published`) toàn bộ ca làm việc trong phạm vi `facilityId` + `fromDate`..`toDate` " +
                          "chỉ bằng 1 lần gọi (1 câu UPDATE duy nhất trong DB, không lặp từng dòng). " +
                          "Các ca không ở trạng thái `Draft` (đã `Published`/`Confirmed`/`Cancelled`) sẽ không bị ảnh hưởng. " +
                          "Trả về `Code = -99` nếu không có ca `Draft` nào trong phạm vi để công bố.",
            OperationId = "PublishBatchWorkSchedule")]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PublishBatch(
            [FromQuery, SwaggerParameter("Id cơ sở", Required = true)] Guid facilityId,
            [FromQuery, SwaggerParameter("Từ ngày", Required = true)] DateTime fromDate,
            [FromQuery, SwaggerParameter("Đến ngày", Required = true)] DateTime toDate,
            [FromQuery, SwaggerParameter("Id người công bố (tuỳ chọn)")] Guid? publishedBy)
        {
            var result = await service.PublishBatchAsync(facilityId, fromDate, toDate, publishedBy);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }


        [HttpPut("{id:guid}/confirm")]
        [SwaggerOperation(
            Summary = "Nhân viên xác nhận lịch làm việc",
            Description = "Chỉ xác nhận được ca đang ở trạng thái `Published`, và `confirmedBy` phải đúng là nhân viên được phân ca đó. " +
                          "Trả về `Code = -99` khi không tìm thấy lịch, lịch chưa được công bố, hoặc không thuộc về nhân viên này.",
            OperationId = "ConfirmWorkSchedule")]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Confirm(
            [SwaggerParameter("Id của lịch làm việc", Required = true)] Guid id,
            [FromBody, SwaggerRequestBody("Nhân viên xác nhận", Required = true)] WorkScheduleConfirmMeta meta)
        {
            var result = await service.ConfirmAsync(id, meta);
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
