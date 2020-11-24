using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace surf_spotter_dot_net_core.Models.Middlewares
{
    public class ConverterMiddleware
    {
        // Delegate to point to the next part of the pipeline
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ConverterMiddleware(RequestDelegate next, ILoggerFactory logFactory)
        {
            _next = next;
            _logger = logFactory.CreateLogger("MyMiddleware");
        }

        // Records the current DateTime for the request to the console 
        // Ability to compare when when requests are made to the side
        public async Task Invoke(HttpContext httpContext)
        {
            DateTime time = DateTime.Now;
            Console.WriteLine("Timestamp THIS is the new request: " + time);
            await _next(httpContext);
        }
    }

    // Extension class to use in the request pipeline
    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ConverterMiddleware>();
        }
    }
}

