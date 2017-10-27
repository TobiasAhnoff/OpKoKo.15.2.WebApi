using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OpKokoDemo.Exceptions;
using OpKokoDemo.Results;

namespace OpKokoDemo.Filters
{
    public class ModelStateValidatorFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = CreateErrorResult(context);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        private static IActionResult CreateErrorResult(ActionContext context)
        {
            var errors = context.ModelState.Values
                .Where(v => v.ValidationState == ModelValidationState.Invalid)
                .SelectMany(v => v.Errors)
                .Select(error => new Error
                {
                    Reason = ErrorTexts.ValidationErrorCode,
                    Message = error.Exception?.Message ?? error.ErrorMessage,
////                    Property = error.
                }).ToList();

            return new ModelStateErrorObjectResult(errors);
        }
    }
}
