using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace OpKokoDemo.Middleware
{
    public static class ExceptionHandlerExtension
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }

    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var error = httpContext.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

            if (error?.Error != null)
            {
                // TODO: Log error

                httpContext.Response.StatusCode = 500;
                httpContext.Response.ContentType = "application/json";

                await httpContext.Response.WriteAsync(
                    JsonConvert.SerializeObject(
                        new {message = $"Internal server error - Contact a system administrator."}));
            }
            else
            {
                // We're not trying to handle anything else so just let the default 
                // handler handle.

                await _next.Invoke(httpContext);
            }
        }
    }
}
