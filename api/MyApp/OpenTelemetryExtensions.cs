using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

public static class OpenTelemetryExtension
{
    private static ResourceBuilder _resourceBuilder = ResourceBuilder
                .CreateDefault()
                .AddService("MyApp")
                .AddTelemetrySdk()
                .AddEnvironmentVariableDetector();

    public static void AddOpenTelemetry(this IServiceCollection services)
    {
        services.AddOpenTelemetryTracing(telemetry =>
        {
            telemetry
                .SetResourceBuilder(_resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .SetSampler(new AlwaysOnSampler())
                .AddOtlpExporter();
        });

        services.AddSingleton<Tracer>(TracerProvider.Default.GetTracer("MyApp"));
    }

    public static void AddOpenTelemetryLogging(this ILoggingBuilder logging)
    {
        logging
            .ClearProviders()
            .AddConsole()
            .AddOpenTelemetry(telemetry =>
            {
                telemetry.IncludeFormattedMessage = true;

                telemetry
                    .SetResourceBuilder(_resourceBuilder)
                    .AttachLogsToActivityEvent()
                    .AddOtlpExporter();
            });
    }
}