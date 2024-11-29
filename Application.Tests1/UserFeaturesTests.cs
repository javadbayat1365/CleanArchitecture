using Application.Contracts.User;
using Application.Features.User.Commands.Register;
using Bogus;
using Domain.Entities.User;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

namespace Application.Tests;

public class UserFeaturesTests
{
    [Fact]
    public async Task Creating_New_User_Should_Be_Success()
    {
        //Arrange
        var faker = new Faker();
        var password = faker.Random.String(10);
        var registerUserRequest = new RegisterUserCommand(
            faker.Person.FirstName,
            faker.Person.LastName,
            faker.Person.UserName,
            faker.Person.Email,
            faker.Person.Phone,
            password,
            password);

        var userManager = Substitute.For<IUserManager>();

        userManager
            .CreateByPasswordAsync(Substitute.For<UserEntity>().ReceivedWithAnyArgs(),password, CancellationToken.None)
            .Returns(IdentityResult.Success);
        //Act

        var userRegisterCommandHandler = new RegisterUserCommandHandler(userManager);

        var userRegisterResult = await userRegisterCommandHandler.Handle(registerUserRequest,CancellationToken.None);

        userRegisterResult.IsSuccess.Should().BeTrue();
        
    }
}
