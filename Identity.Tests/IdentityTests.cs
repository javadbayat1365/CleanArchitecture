using Application.Features.User.Commands.Register;
using Application.Features.User.Queries.PasswordLogin;
using FluentAssertions;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Tests;

public class IdentityTests:IClassFixture<IdentityTestSetup>
{
    private IServiceProvider _serviceProvider;
    public IdentityTests(IdentityTestSetup setup)
    {
        _serviceProvider = setup.serviceProvider;
    }

    [Fact]
    public async Task Registering_User_Should_Success()
    {
        var testUser = new RegisterUserCommand("Test", "Test", "Test", "Test@gmail.com","09369364556","pw123321", "pw123321");
        var sender = _serviceProvider.GetRequiredService<ISender>();

        var registerResult = await sender.Send(testUser);

        registerResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Getting_Access_Token_Should_Success()
    {
        var testUser = new RegisterUserCommand("Test", "Test", "Test2", "Test2@gmail.com", "09369364556", "pw123321", "pw123321");
        var sender = _serviceProvider.GetRequiredService<ISender>();

         await sender.Send(testUser);

        var tokenQuery = new UserPasswordLoginQuery("Test2","pw123321");

        var tokenQueryResult = await sender.Send(tokenQuery);

        tokenQueryResult.Result!.AccessToken.Should().NotBeEmpty();
    }
}
