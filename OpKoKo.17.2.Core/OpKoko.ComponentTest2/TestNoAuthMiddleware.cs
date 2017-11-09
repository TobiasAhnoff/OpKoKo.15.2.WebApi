using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace OpKoko.ComponentTest
{
    public static class TestNoAuthMiddlewareExtension
    {
        public static IApplicationBuilder UseTestNoAuthMiddleware(this IApplicationBuilder builder, string devloperSub, string devloperScope) {
            return builder.UseMiddleware<TestNoAuthMiddleware>(devloperSub, devloperScope);
        }
    }

    public class TestNoAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _devloperSub;
        private readonly string _devloperScope;

        public TestNoAuthMiddleware(RequestDelegate next, string devloperSub, string devloperScope) {
            _next = next;
            _devloperSub = devloperSub;
            _devloperScope = devloperScope;
        }

        public async Task Invoke(HttpContext context) {
            var developerClaims = new List<Claim>()
            {
                new Claim("sub", _devloperSub, ClaimValueTypes.String, "TestNoAuthMiddleware"),
                new Claim("scope", _devloperScope, ClaimValueTypes.String, "TestNoAuthMiddleware")
            };

            var claimsIdentity = new ClaimsIdentity(developerClaims, "TestNoAuthMiddleware");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            context.User = claimsPrincipal;

            await _next(context);
        }
    }
}

