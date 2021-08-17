using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebAPI.Middleware
{
    public class ForceJsonMiddleware
    {
        private static readonly List<string> forceJsonRoutes = new List<string> {
            "/api/applicantCv/webhook/parsed",
        };

        private readonly RequestDelegate _next;

        public ForceJsonMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (forceJsonRoutes.Contains(context.Request.Path.Value))
            {
                context.Request.ContentType = "application/json";
            }

            await _next(context);
        }
    }
}
