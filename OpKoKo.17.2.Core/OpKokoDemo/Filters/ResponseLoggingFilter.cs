using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using OpKokoDemo.Responses;
using Serilog;

namespace OpKokoDemo.Filters
{
    public class ResponseLoggingFilter : IResultFilter
    {
        private const int MaxBodyLength = 10000;

        public void OnResultExecuting(ResultExecutingContext context)
        {
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.Exception != null)
                return;

            var body = GetResponseBody(context);
            var text = GetActionAttributeText(context.ActionDescriptor);
            var statusCode = GetStatusCode(context);
            Log(context.HttpContext, $"[Completed] [{text}] [{context.HttpContext.Request.Method} to '{context.HttpContext.Request.Path}']", body, statusCode);
        }

        private static string GetResponseBody(ResultExecutedContext context)
        {
            try
            {
                var result = context.Result as ObjectResult;

                var body = result?.Value as Response;

                var response = body == null ? string.Empty : JsonConvert.SerializeObject(body, Formatting.Indented);
                return response.Length > MaxBodyLength ? response.Substring(0, MaxBodyLength) + "... [clipped]" : response;
            }
            catch (Exception)
            {
                /* ignored */
            }

            return string.Empty;
        }

        private static string GetActionAttributeText(ActionDescriptor actionDescriptor)
        {
            var controllerActionDescriptor = actionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null)
                return string.Empty;

            var attribute = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                .FirstOrDefault(a => a.GetType() == typeof(DescriptionAttribute));

            var requestResponseLoggingFilterNameAttribute = attribute as DescriptionAttribute;

            return requestResponseLoggingFilterNameAttribute?.Description ?? string.Empty;
        }

        private static string GetStatusCode(ActionContext context)
        {
            return context.HttpContext.Response.StatusCode.ToString();
        }

        private static void Log(HttpContext context, string message, string body, string statusCode)
        {
            GetLogger(context)
                .Information($"{message} {body} {statusCode}");

            ////GetLogger(context)
            ////    .ForContext("Payload", body)
            ////    .ForContext("StatusCode", statusCode)
            ////    .Information(message);
        }

        private static ILogger GetLogger(HttpContext context) => ((ILogger)context.RequestServices.GetService(typeof(ILogger))).ForContext<ResponseLoggingFilter>();
    }
}
