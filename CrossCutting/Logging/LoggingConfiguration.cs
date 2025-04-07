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
            configuration.WriteTo.Seq(context.Configuration["Seq:Url"]!,Serilog.Events.LogEventLevel.Information,
                apiKey: context.Configuration["Seq:ApiKey"]);
            return;
        }
        if (context.HostingEnvironment.IsProduction())
        {
            configuration.WriteTo.Console().MinimumLevel.Error();
            //TODO Run SEQ On Docker with 5341 port localhost
            configuration.WriteTo.Seq(
               serverUrl: context.Configuration["Seq:Url"]!,
               apiKey: context.Configuration["Seq:ApiKey"],
               restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information);
        }
    };
}
