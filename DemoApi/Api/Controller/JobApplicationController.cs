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
    [Route("api/v1/job-applications")]
    [SwaggerTag("Quản lý danh mục CV")]
    public class JobApplicationControlleṛ(IJobApplicationService service) : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(
       Summary = "Danh sách các CV",
       Description = "Trả về toàn bộ danh sách CV",
       OperationId = "GetListJobApplication")]
        [ProducesResponseType(typeof(ActionResultResponse<List<JobApplicationViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] JobApplicationSearchMeta search)
        {
            var result = await service.GetListAsync(search);
            return Ok(result);
        }

        [HttpGet("{id:guid}/job-positions")]
        [SwaggerOperation(
        Summary = "Danh sách CV theo vị trí công việc",
        Description = "Trả về toàn bộ CV theo vị trí công việc",
        OperationId = "GetListJobApplicationByJobApplicationId")]
        [ProducesResponseType(typeof(ActionResultResponse<List<JobApplicationViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListJobApplicationByJobApplicationId([SwaggerParameter("Id của vị trí công việc", Required = true)] Guid id)
        {
            var result = await service.GetListJobApplicationByJobApplicationId(id);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [SwaggerOperation(
            Summary = "Chi tiết một CV ứng tuyển",
            Description = "Trả về `Code = -99` (HTTP 404) khi không tìm thấy.",
            OperationId = "GetJobApplicationDetail")]
        [ProducesResponseType(typeof(ActionResultResponse<EducationLevelViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetail(
            [SwaggerParameter("Id của CV ứng tuyển", Required = true)] Guid id)
        {
            var result = await service.GetDetailAsync(id);
            return result.Code <= 0 ? NotFound(result) : Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Nộp CV ứng tuyển",
            Description = "Trả về `Code = 0` nếu nộp thất bại",
            OperationId = "ApplyJobApplication")]
        [ProducesResponseType(typeof(ActionResultResponse<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody, SwaggerRequestBody("Thông tin CV cần nộp", Required = true)] JobApplicationMeta meta)
        {
            var result = await service.CreateAsync(meta);
            return result.Code <= 0 ? BadRequest(result) : Ok(result);
        }

        
        //[HttpDelete("{id:guid}")]
        //[SwaggerOperation(
        //    Summary = "Xoá (mềm) trình độ học vấn",
        //    Description = "Trả về `Code = -99` (HTTP 400) khi không tìm thấy.",
        //    OperationId = "DeleteEducationLevel")]
        //[ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ActionResultResponse), StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> Delete(
        //    [SwaggerParameter("Id của trình độ học vấn", Required = true)] Guid id)
        //{
        //    var result = await service.DeleteAsync(id);
        //    return result.Code <= 0 ? BadRequest(result) : Ok(result);
        //}

    }
}
