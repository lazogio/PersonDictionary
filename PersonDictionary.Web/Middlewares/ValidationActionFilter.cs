using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PersonDictionary.Middlewares
{
    public class ValidationActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var response = new BadRequestResponse("One or more validation errors occurred.");
                context.Result = new BadRequestObjectResult(new
                {
                    Error = response.Message
                });
                return;
            }
            await next();
        }
    }
    public class BadRequestResponse : IRequest<IActionResult>
    {
        public string Message { get; }
        public BadRequestResponse(string message)
        {
            Message = message;
        }
    }
}
