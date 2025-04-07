using Application.Extensions;
using Clean.WebFramework.Extensions;
using Clean.WebFramework.Swagger;
using CrossCutting.FileStorageService.Extensions;
using CrossCutting.Logging;
using Identity.Extensions;
using Persistence;
using Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

//builder.Host.UseSerilog(LoggingConfiguration.ConfigureLogger);
// Add services to the container.

builder
    .AddVersioning()
    .ConfigureAuthenticationAndAuthorization()
    .AddSwagger(new string[] {"V1"});

builder.Services
    .AddIdentityServices(builder.Configuration)
    .AddFileService(builder.Configuration)
    .AddApplicationAutomapper()
    .AddApplicationMediatorServices()
    .RegisterApplicationValidators()
    .AddPersistenceDbContext(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    //app.mi
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
