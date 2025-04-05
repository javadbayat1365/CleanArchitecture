using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;

namespace CrossCutting.Logging;

public class LoggingConfiguration
{
    public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger = (context, configuration) =>
    {

        var applicationName = context.HostingEnvironment.ApplicationName;

        configuration
        .Enrich.WithSpan()
        .Enrich.WithEnvironmentName()
        .Enrich.WithMachineName()
        .Enrich.WithExceptionDetails()
        .Enrich.WithProperty("ApplicationName: ", applicationName);

        if (context.HostingEnvironment.IsDevelopment())
        {
            configuration.WriteTo.Console().MinimumLevel.Information();
            return;
        }
        if (context.HostingEnvironment.IsProduction())
        {
            configuration.WriteTo.Console().MinimumLevel.Error();
            //TODO Run SEQ On Docker with 5341 port localhost
            configuration.WriteTo.Seq(
               serverUrl: "localhost:5341",
               apiKey: "jl5k34j34j5lj34l5jl",
               restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information);
        }
    };
}
