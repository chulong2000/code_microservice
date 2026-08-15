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
        
    }
}
