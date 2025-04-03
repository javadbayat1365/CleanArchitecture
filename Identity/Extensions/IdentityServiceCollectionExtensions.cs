using Domain.Entities.User;
using Identity.IdentitySetup.Factories;
using Identity.IdentitySetup.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Identity.Extensions;

public static class IdentityServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddScoped<IUserClaimsPrincipalFactory<UserEntity>,AppUserClaimPrincipalFactory>();
        services.AddScoped<IRoleStore<RoleEntity>,AppRoleStore>();
        services.AddScoped<IUserStore<UserEntity>,AppUserStore>();

        services.AddIdentity<UserEntity, RoleEntity>(options =>
        {

            options.Stores.ProtectPersonalData = false;

            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredUniqueChars = 0;

            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = false;

            options.User.RequireUniqueEmail = false;

        }).AddRoleStore<AppRoleStore>()
        .AddUserStore<AppUserStore>()
        .AddClaimsPrincipalFactory<AppUserClaimPrincipalFactory>()
        .AddDefaultTokenProviders()
        .AddEntityFrameworkStores<CleanDbContext>();

        return services;
    }
}
