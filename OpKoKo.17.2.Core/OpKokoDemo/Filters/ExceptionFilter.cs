using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using OpKokoDemo.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OpKokoDemo.Filters
{
    public class ExceptionFilter : IActionFilter
    {
        private readonly Dictionary<Type, Func<Exception, ExceptionResult>> _handlers = new Dictionary<Type, Func<Exception, ExceptionResult>>
        {
            { typeof(ArgumentException), e => new ExceptionResult(ErrorTexts.ValidationErrorCode, e.Message, 400) },
            { typeof(BadRequestException), e => new ExceptionResult(ErrorTexts.BadRequestErrorCode, e.Message, 400) },
            { typeof(NotImplementedException), e => new ExceptionResult(ErrorTexts.NotImplementedCode, ErrorTexts.NotImplementedMessage, 501) }
            
        };

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
                return;

            var exception = GetInnermostHandledException(context.Exception);
            var isHandled = exception != null;
            var handler = isHandled ? _handlers[exception.GetType()] : _ => new ExceptionResult();

            var errorResult = handler.Invoke(exception);

            context.Result = new ObjectResult(errorResult)
            {
                StatusCode = errorResult.HttpStatusCode
            };

            var logger = GetLogger(context.HttpContext);

            if (isHandled)
                logger.Warning(context.Exception, "A Handled Exception occurred.");
            else
                logger.Error(context.Exception, "An Unhandled Exception occured.");

            context.ExceptionHandled = true;
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

        private static Serilog.ILogger GetLogger(HttpContext context) => ((Serilog.ILogger)context.RequestServices.GetService(typeof(Serilog.ILogger))).ForContext<ExceptionFilter>();
    }
}
