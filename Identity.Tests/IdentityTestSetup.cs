namespace Identity.Tests;

using Application.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Testcontainers.MsSql;
using Identity.Extensions;
using Microsoft.Extensions.Logging;

public class IdentityTestSetup: IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder().Build();//Hosted Db On Docker

    public IServiceProvider serviceProvider { get; private set; }

    public async Task DisposeAsync()
    {
        await _msSqlContainer.StopAsync();
    }

    public async Task InitializeAsync()
    {
        try
        {
          await _msSqlContainer.StartAsync();
        }
        catch (Exception)
        {
            var logs = await _msSqlContainer.GetLogsAsync();
            Console.WriteLine($"Container logs : {logs.Stderr} -- {logs.Stdout}");
            throw;
        }

        var dbOptionBuilder = new DbContextOptionsBuilder<CleanDbContext>().UseSqlServer(_msSqlContainer.GetConnectionString());
        var db = new CleanDbContext(dbOptionBuilder.Options);

        await db.Database.MigrateAsync();

        var configs = new Dictionary<string, string>()
        {
            {"ConnectionStrings:CleanDb",_msSqlContainer.GetConnectionString() },
            {"JwtConfiguration:SignInKey","ShouldBe-LongerThan-16Char-SecretKey" },
            {"JwtConfiguration:Audience","TestAud" },
            {"JwtConfiguration:Issuer","TestIssuer" },
            {"JwtConfiguration:ExpirationMinute","60" }
        };

        var configuratoinBuilder = new ConfigurationBuilder();

        var inMemoryConfigs = new MemoryConfigurationSource() { InitialData = configs };

        configuratoinBuilder.Add(inMemoryConfigs);
        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddApplicationMediatorServices()
            .RegisterApplicationValidators()
            .AddLogging(builder => builder.AddConsole())
            .AddIdentityServices(configuratoinBuilder.Build());
            //.AddApplicationAutoMapper()

        serviceProvider = serviceCollection.BuildServiceProvider(false);
    }
}
