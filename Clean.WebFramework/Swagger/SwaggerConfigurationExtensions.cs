using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag.Generation.Processors.Security;

namespace Clean.WebFramework.Swagger;

public static class SwaggerConfigurationExtensions
{
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder, string[] versions)
    {
        foreach (var version in versions) { 
        
            builder.Services.AddSwaggerDocument(options =>
            {
                options.Title = "Clean API";
                options.Version = version;
                options.DocumentName = version;

                options.AddSecurity("Bearer",new NSwag.OpenApiSecurityScheme() { 
                 Description="Enter JWT Token Only",
                 In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                 Type = NSwag.OpenApiSecuritySchemeType.Http,
                 Scheme = "Bearer"
                });

                options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
                options.DocumentProcessors.Add(new ApiVersionDocumentProcessor());
            });
        

        }

        return builder;
    }
    public static WebApplication UseSwagger(this WebApplication app)
    {
        if(app.Environment.IsProduction())
            return app;

        app.UseOpenApi();
        app.UseSwaggerUi(options => {
            options.PersistAuthorization = true;
            options.EnableTryItOut = true;
            options.Path = "/swagger";
        });

        app.UseReDoc(settings => {
            settings.Path = "/api-doces/{documentName}";
            settings.DocumentTitle = "Clean API Documentation";
        });

        return app;
    }
}
