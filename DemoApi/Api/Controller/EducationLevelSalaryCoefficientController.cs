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
    [Route("api/v1/education-level-salary-coefficient")]
    [SwaggerTag("Quản lý phụ cấp theo trình độ học vấn")]
    public class EducationLevelSalaryCoefficientController(IEducationLevelSalaryCoefficientService service) : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(
       Summary = "Danh sách toàn bộ hệ số lương",
       Description = "Trả về toàn bộ danh sách hệ số lương",
       OperationId = "GetEducationLevelSalaryCoefficient")]
        [ProducesResponseType(typeof(ActionResultResponse<List<EducationLevelViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList()
        {
            var result = await service.GetListAsync();
            return Ok(result);
        }

        [HttpGet("/education-levels/{educationLevelId}/salary-coefficient")]
        [SwaggerOperation(
        Summary = "Hệ số lương của 1 trình độ học vấn",
        Description = "Trả về hệ số lương của 1 trình độ học vấn",
        OperationId = "")]
        [ProducesResponseType(typeof(ActionResultResponse<List<EducationLevelSalaryCoefficientViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSalaryCoefficientByEducationLevel(Guid educationLevelId)
        {
            var result = await service.GetSalaryCoefficientByEducationLevelId(educationLevelId);
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Thê",
            Description = "Trả về `Code = -1` (HTTP 400) khi tên đã tồn tại.",
            OperationId = "CreateEducationLevel")]
        [ProducesResponseType(typeof(ActionResultResponse<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody, SwaggerRequestBody("Thông tin hệ số lương", Required = true)] EducationLevelSalaryCoefficientMeta meta)
        {
            var result = await service.CreateAsync(meta);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }


        [HttpPut("{id:guid}")]
        [SwaggerOperation(
            Summary = "Tạo trình độ học vấn",
            Description = "Trả về `Code = -1` (HTTP 400) khi tên đã tồn tại.",
            OperationId = "CreateEducationLevel")]
        [ProducesResponseType(typeof(ActionResultResponse<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(
            [FromBody, SwaggerRequestBody("Cập nhập hệ số lương", Required = true)] EducationLevelSalaryCoefficientMeta meta)
        {
            var result = await service.UpdateAsync(meta);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [SwaggerOperation(
            Summary = "Xoá (mềm) hệ số lương",
            Description = "Trả về `Code = -99` (HTTP 400) khi không tìm thấy.",
            OperationId = "Delete Education Level Salary Coefficient")]
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
