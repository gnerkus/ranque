using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Entities.Test
{
    internal class AutoAuthorizeStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                builder.UseMiddleware<AutoAuthorizeMiddleware>();
                next(builder);
            };
        }
    }

    internal class AutoAuthorizeMiddleware
    {
        private readonly RequestDelegate _rd;

        public AutoAuthorizeMiddleware(RequestDelegate rd)
        {
            _rd = rd;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var identity = new ClaimsIdentity("Bearer");

            identity.AddClaim(new Claim("sub", "1234567"));
            identity.AddClaim(new Claim(ClaimTypes.Name, "test-name"));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier,
                "18e62a3b-f503-4d2f-9b22-9f7a6f0ede10"));

            identity.AddClaims(GetClaimsBasedOnHttpHeaders(httpContext));

            httpContext.User.AddIdentity(identity);
            await _rd.Invoke(httpContext);
        }

        /// <summary>
        ///     add claims to the identity based on the headers in the request
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static List<Claim> GetClaimsBasedOnHttpHeaders(HttpContext context)
        {
            const string headerPrefix = "X-Test-";

            var claims = new List<Claim>();

            var claimHeaders = context.Request.Headers.Keys.Where(k => k.StartsWith(headerPrefix));
            foreach (var header in claimHeaders)
            {
                var value = context.Request.Headers[header];
                var claimType = header[headerPrefix.Length..];
                if (!string.IsNullOrEmpty(value))
                    claims.Add(
                        new Claim(claimType == "role" ? ClaimTypes.Role : claimType, value!));
            }

            return claims;
        }
    }
}