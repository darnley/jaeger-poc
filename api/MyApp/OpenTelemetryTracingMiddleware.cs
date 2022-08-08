using OpenTelemetry.Trace;

namespace MyApp
{
    public class OpenTelemetryTracingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.Headers.Add("X-Trace-Id", Tracer.CurrentSpan.Context.TraceId.ToString());

            await next.Invoke(context);
        }
    }
}
