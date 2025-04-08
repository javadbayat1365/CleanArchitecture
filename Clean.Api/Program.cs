using Application.Extensions;
using Clean.WebFramework.Extensions;
using Clean.WebFramework.Filters;
using Clean.WebFramework.Models;
using Clean.WebFramework.Swagger;
using CrossCutting.FileStorageService.Extensions;
using CrossCutting.Logging;
using Identity.Extensions;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

//builder.Host.UseSerilog(LoggingConfiguration.ConfigureLogger);
// Add services to the container.

builder
    .AddSwagger(new string[] { "V1" })
    .AddVersioning();

builder.Services
    .AddIdentityServices(builder.Configuration)
    .AddFileService(builder.Configuration)
    .AddApplicationAutomapper()
    .AddApplicationMediatorServices() 
    .RegisterApplicationValidators()
    .AddPersistenceDbContext(builder.Configuration);

builder.ConfigureAuthenticationAndAuthorization();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(OkResultAttribute));
    options.Filters.Add(typeof(NotFoundAttribute));
    options.Filters.Add(typeof(BadRequestAttribute));
    options.Filters.Add(typeof(ModelStateValidationAttribute));

    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ApiResult<Dictionary<string, List<string>>>), StatusCodes.Status400BadRequest));
    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ApiResult), StatusCodes.Status401Unauthorized));
    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ApiResult), StatusCodes.Status403Forbidden));
    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ApiResult), StatusCodes.Status500InternalServerError));
}).ConfigureApiBehaviorOptions(options => { 
   options.
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    await app.ApplyMigrationAsync();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
