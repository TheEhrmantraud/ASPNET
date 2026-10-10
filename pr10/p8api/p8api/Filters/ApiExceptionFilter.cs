using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using p8api.Exceptions;

namespace p8api.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            IActionResult? result = context.Exception switch
            {
                RelatedEntityNotFoundException ex => new BadRequestObjectResult(new { error = ex.Message }),
                BusinessRuleException ex => new ConflictObjectResult(new { error = ex.Message }),
                DbUpdateException => new ConflictObjectResult(new { error = "Операция нарушает ограничения целостности данных." }),
                _ => null
            };

            if (result is null)
            {
                return;
            }

            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}
