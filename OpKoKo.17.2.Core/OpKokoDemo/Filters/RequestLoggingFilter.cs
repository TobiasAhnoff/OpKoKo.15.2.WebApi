using System.ComponentModel;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace OpKokoDemo.Filters
{
    public class RequestLoggingFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var text = GetActionAttributeText(context.ActionDescriptor);
            var body = GetFormattedBody(context.HttpContext);
            var queryString = GetQueryString(context.HttpContext);
            Log(context.HttpContext, $"[Initiated] [{text}] [{context.HttpContext.Request.Method} to '{context.HttpContext.Request.Path}']", body, queryString);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
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

        private static string GetFormattedBody(HttpContext context)
        {
            var rawRequestBody = GetRequestBody(context);

            return string.IsNullOrEmpty(rawRequestBody) ? string.Empty : GetFormattedJsonBody(rawRequestBody);
        }

        private static string GetRequestBody(HttpContext context)
        {
            try
            {
                context.Request.Body.Seek(0, SeekOrigin.Begin);
                return new StreamReader(context.Request.Body).ReadToEnd();
            }
            catch (System.Exception)
            {
                /* Ignore */
            }

            return string.Empty;
        }

        private static string GetFormattedJsonBody(string body)
        {
            try
            {
                return JToken.Parse(body).ToString(Formatting.Indented);
            }
            catch (JsonReaderException)
            {
                return body;
            }
        }

        private static string GetQueryString(HttpContext contextHttpContext)
        {
            var parameters = contextHttpContext.Request.Query.ToDictionary(k => k.Key, k => k.Value.ToString());
            return JsonConvert.SerializeObject(parameters, Formatting.Indented);
        }

        private static void Log(HttpContext context, string message, string body, string queryString)
        {
            GetLogger(context)
                .Information($"{message} {body} {queryString}");
            ////GetLogger(context)
            ////    .ForContext("Payload", body)
            ////    .ForContext("QueryString", queryString)
            ////    .Information(message);
        }

        private static ILogger GetLogger(HttpContext context) => ((ILogger)context.RequestServices.GetService(typeof(ILogger))).ForContext<RequestLoggingFilter>();
    }
}
