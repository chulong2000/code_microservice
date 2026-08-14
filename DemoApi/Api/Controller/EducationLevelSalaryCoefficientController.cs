using DemoApi.Domain.IServices;
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
       Summary = "Danh sách phụ cấp theo trình độ học vấn",
       Description = "Trả về toàn bộ danh mục phụ cấp theo trình độ học vấn.",
       OperationId = "GetEducationLevels")]
        [ProducesResponseType(typeof(ActionResultResponse<List<EducationLevelSalaryCoefficientViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList()
        {
            var result = await service.GetListAsync();
            return Ok(result);
        }
    }
}
