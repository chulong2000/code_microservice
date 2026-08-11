using GHM.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;

namespace GHM.Infrastructure.CustomAttributes
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                //actionContext.Result = new BadRequestObjectResult(actionContext.ModelState);
                actionContext.Result = new BadRequestObjectResult(new
                {
                    Code = HttpStatusCode.BadRequest,
                    Title = actionContext.ModelState.Values.Distinct().Select(e => e.Errors.Select(er => er.ErrorMessage)),
                    Message = actionContext.ModelState.SelectMany(state => state.Value.Errors).Aggregate("", (current, error) => current + (error.ErrorMessage)),
                    Data = actionContext.ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                });
            }
        }
    }

    public class ModelStateFilter : IActionFilter
    {
        public string ConvertEmptyStringToNull { get; set; }
        public void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                //actionContext.Result = new BadRequestObjectResult(actionContext.ModelState);

                //actionContext.Result = new BadRequestObjectResult(new
                //{
                //    Data = ConvertEmptyStringToNull,
                //    Title = "",
                //    Message = actionContext.ModelState.SelectMany(state => state.Value.Errors).Select(x => x.ErrorMessage).FirstOrDefault(),
                //    // Message = actionContext.ModelState.SelectMany(state => state.Value.Errors).Aggregate("", (current, error) => current + (error.ErrorMessage)),
                //    // Message = actionContext.ModelState.Values.SelectMany(x => x.Errors.Select(p => p.ErrorMessage)).ToList(),
                //    Code = HttpStatusCode.BadRequest,
                //});

                actionContext.Result = new BadRequestObjectResult(new ActionResultResponse<dynamic>(-1,actionContext.ModelState.SelectMany(state => state.Value.Errors).Select(x => x.ErrorMessage).FirstOrDefault(),string.Empty,actionContext.ModelState.Values.SelectMany(x => x.Errors.Select(p => p.ErrorMessage)).ToList()));

            }

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
