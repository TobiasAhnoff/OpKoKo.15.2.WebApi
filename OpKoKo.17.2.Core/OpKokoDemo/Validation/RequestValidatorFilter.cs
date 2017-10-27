using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OpKokoDemo.Models;

namespace OpKokoDemo.Validation
{
    public class RequestValidatorFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var validatableRequests = context.ActionArguments.Values.Where(v => v is IValidatableRequest).ToList();

            if (!validatableRequests.Any())
            {
                return;
            }

            foreach (var request in validatableRequests)
            {
                var language = (Language)request.GetType().GetProperty("Language").GetValue(request, null);
                var requestType = request.GetType();
                var validator = GetValidatorFor(context, requestType, language);

                if (validator == null)
                    continue;

                ValidateRequest(context, validator, (IValidatableRequest)request);
            }
        }

        private static void ValidateRequest(ActionContext context, IRequestValidator validator, IValidatableRequest request)
        {
            var validationErrors = validator.Validate(request);
            foreach (var validationError in validationErrors)
            {
                context.ModelState.TryAddModelError(validationError.PropertyName, validationError.ErrorMessage);
            }
        }

        private static IRequestValidator GetValidatorFor(ActionContext context, Type requestType, Language language)
        {
            var validators = (IEnumerable<IRequestValidator>)context.HttpContext.RequestServices.GetService(typeof(IEnumerable<IRequestValidator>));

            var validatorsPerRequest = validators.Where(v => v.RequestType == requestType).ToList();

            if (!validatorsPerRequest.Any())
                return null;

            var validator = validatorsPerRequest.SingleOrDefault(v => v.ValidFor(language));

            if (validator != null)
                return validator;

            throw new NotImplementedException($"Missing validator for {requestType.Name}");
        }
    }
}
