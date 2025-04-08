using Application.Contracts.FileService.Interfaces;
using CrossCutting.FileStorageService.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace CrossCutting.FileStorageService.Extensions;

public static class FileStorageServiceCollectionExtensions
{
    public static IServiceCollection AddFileService(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddMinio(options => {
            options.WithCredentials(accessKey: configuration["Minio:accessKey"], secretKey: configuration["Minio:secretKey"])
            .WithEndpoint(configuration["Minio:Endpoint"])
            .WithSSL(configuration.GetValue<bool>("Minio:UseSsl"));
        });

        services.AddScoped<IFileService, MinioStorageService>();


        services.AddKeyedScoped<IMinioClient>("SasMiniioClient", (serviceProvicer,_) => {
            var client = new MinioClient();
            return client.WithEndpoint(configuration["Minio:SasEndpoint"])
            .WithCredentials(configuration["Minio:AccessKey"], configuration["Minio:SecretKey"])
            .WithSSL(configuration.GetValue<bool>("Minio:UseSsl"));
        });

        return services;
    }
}
