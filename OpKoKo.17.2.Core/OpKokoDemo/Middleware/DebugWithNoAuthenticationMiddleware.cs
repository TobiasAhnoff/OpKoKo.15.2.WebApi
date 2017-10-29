using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace OpKokoDemo.Middleware
{
    public static class DebugWithNoAuthenticationMiddlewareExtension
    {
        public static IApplicationBuilder UseDebugWithNoAuthentication(this IApplicationBuilder builder, string devloperSub, string devloperScope)
        {
            return builder.UseMiddleware<DebugWithNoAuthenticationMiddleware>(devloperSub, devloperScope);
        }
    }

    public class DebugWithNoAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _devloperSub;
        private readonly string _devloperScope;

        public DebugWithNoAuthenticationMiddleware(RequestDelegate next, string devloperSub, string devloperScope)
        {
            _next = next;
            _devloperSub = devloperSub;
            _devloperScope = devloperScope;
        }

        public async Task Invoke(HttpContext context)
        {
            var developerClaims = new List<Claim>()
            {
                new Claim("sub", _devloperSub, ClaimValueTypes.String, "DebugWithNoAuthenticationMiddleware"),
                new Claim("scope", _devloperScope, ClaimValueTypes.String, "DebugWithNoAuthenticationMiddleware")
            };

            var claimsIdentity = new ClaimsIdentity(developerClaims, "DebugWithNoAuthentication");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            context.User = claimsPrincipal;

            await _next(context);
        }
    }
}
