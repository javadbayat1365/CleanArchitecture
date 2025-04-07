using Asp.Versioning;
using Clean.WebFramework.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Clean.WebFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddVersioning(this WebApplicationBuilder builder)
    {
        builder.Services.AddApiVersioning(options => {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1,0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddMvc().AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        return builder;
    }

    public static WebApplicationBuilder ConfigureAuthenticationAndAuthorization(this WebApplicationBuilder builder)
    {
        var signKey = builder.Configuration.GetSection("JwtConfiguration")["SignInKey"]!;
        var encryptionKey = builder.Configuration.GetSection("JwtConfiguration")["EncryptionKey"]!;
        var issuer = builder.Configuration.GetSection("JwtConfiguration")["Issuer"]!;
        var audience = builder.Configuration.GetSection("JwtConfiguration")["Audience"]!;


        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var validationParameters = new TokenValidationParameters() { 

             ClockSkew = TimeSpan.Zero,
             RequireSignedTokens = true,

             ValidateIssuerSigningKey = true,
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey)),

             ValidateIssuer = true,
             ValidIssuer = issuer,

             ValidateAudience =true,
             ValidAudience = audience,

             TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(encryptionKey))
            };
            options.TokenValidationParameters = validationParameters;

            options.Events = new JwtBearerEvents() { 
             OnForbidden = async context => {
                 context.Response.StatusCode = StatusCodes.Status403Forbidden;
                 await context.Response.WriteAsJsonAsync(new ApiResult(false, "Forbidden", ApiResultStatusCode.Forbidden));
              }
            };
        });

        return builder;
    }
}
