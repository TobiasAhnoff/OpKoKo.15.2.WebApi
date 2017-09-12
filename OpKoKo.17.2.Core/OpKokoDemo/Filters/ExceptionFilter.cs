using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using OpKokoDemo.Exceptions;

namespace OpKokoDemo.Filters
{
    public class ExceptionFilter : IActionFilter
    {
        private readonly Dictionary<Type, Func<Exception, ExceptionResult>> _handlers = new Dictionary<Type, Func<Exception, ExceptionResult>>
        {
            { typeof(ArgumentException), e => new ExceptionResult(ErrorTexts.ValidationErrorCode, e.Message, 400) },
            { typeof(BadRequestException), e => new ExceptionResult(ErrorTexts.BadRequestErrorCode, e.Message, 400) },
        };

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
                return;

            var exception = GetInnermostHandledException(context.Exception);
            var isHandled = exception != null;
            var handler = isHandled ? _handlers[exception.GetType()] : _ => new ExceptionResult();

            var result = handler.Invoke(exception);
            context.Result = result;
            context.ExceptionHandled = true;

            //var loggerFactory = (ILogger)context.HttpContext.RequestServices.GetService(typeof(ILogger));

            //var logger = loggerFactory.ForContext<ExceptionFilter>();

            //if (isHandled)
            //    logger.Warning(context.Exception, "A Handled Exception occurred.");
            //else
            //    logger.Error(context.Exception, "An Unhandled Exception occured.");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        private Exception GetInnermostHandledException(Exception exception)
        {
            if (exception.InnerException != null)
            {
                var innerException = GetInnermostHandledException(exception.InnerException);
                if (innerException != null)
                    return innerException;
            }

            return _handlers.ContainsKey(exception.GetType()) ? exception : null;
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return next();
        }
    }
}
