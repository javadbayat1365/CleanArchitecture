using Application.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories.Common;

namespace Persistence.Extensions;

public static class PersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddPersistenceDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CleanDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("CleanDb"), builder =>
            {

                builder.EnableRetryOnFailure();
                builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
            });
        });

        services.AddScoped<IUnitOfWork,UnitOfWork>();

        return services;
    }
}
