using Application.Contracts.User;
using Application.Features.User.Commands.Register;
using Bogus;
using Domain.Entities.User;
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
        var loginUserRequest = new RegisterUserCommand(
            faker.Person.FirstName,
            faker.Person.LastName,
            faker.Person.UserName,
            faker.Person.Email,
            faker.Person.Phone,
            password,
            password);

        var userManager = Substitute.For<IUserManager>();
        var userEntity = new UserEntity();
        userManager.PasswordCreateAsync();
        //Act

        //
    }
}
