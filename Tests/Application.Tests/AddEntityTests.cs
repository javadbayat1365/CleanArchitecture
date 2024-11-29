using Application.Common;
using Application.Contracts.User;
using Application.Features.User.Commands.Register;
using Bogus;
using Domain.Entities.User;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Application.Tests
{
    public class AddEntityTests
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

            //Act
            userManager
                .CreateByPasswordAsync(Arg.Any<UserEntity>(), registerUserRequest.Password, CancellationToken.None)
                .Returns(Task.FromResult(IdentityResult.Success));

            var userRegisterCommandHandler = new RegisterUserCommandHandler(userManager);

            var userRegisterResult = await userRegisterCommandHandler.Handle(registerUserRequest, CancellationToken.None);
            
            //assert
            userRegisterResult.IsSuccess.Should().BeTrue();

        }

        [Fact]
        public async Task Creating_User_Should_Be_False_If_We_Pass_Null_UserName()
        {
            //arrange
            var faker = new Faker();
            var password = faker.Random.String(10);
            var user = new RegisterUserCommand(
                faker.Person.FirstName,
                faker.Person.LastName,
                string.Empty,
                faker.Person.Email,
                faker.Person.Phone,
                password,
                password
                );

            var usermanager = Substitute.For<IUserManager>();
            var handler = new RegisterUserCommandHandler(usermanager);
            usermanager.CreateByPasswordAsync(Arg.Any<UserEntity>(),user.Password,CancellationToken.None)
                              .Returns(Task.FromResult(IdentityResult.Success));
            //act
            var userRegisterResult =await handler.Handle(user, CancellationToken.None);

            //assert
            userRegisterResult.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public async Task Creating_User_Should_Be_False_If_We_Pass_Null_FirstName()
        {
            //arrange
            var faker = new Faker();
            var password = faker.Random.String(10);
            var user = new RegisterUserCommand(
                string.Empty,
                faker.Person.LastName,
                faker.Person.UserName,
                faker.Person.Email,
                faker.Person.Phone,
                password,
                password
                );

            var usermanager = Substitute.For<IUserManager>();
            var handler = new RegisterUserCommandHandler(usermanager);
            usermanager.CreateByPasswordAsync(Arg.Any<UserEntity>(), user.Password, CancellationToken.None)
                              .Returns(Task.FromResult(IdentityResult.Success));
            //act
            var userRegisterResult = await handler.Handle(user, CancellationToken.None);

            //assert
            userRegisterResult.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public async Task Creating_User_Should_Be_False_If_We_Pass_Null_LastName()
        {
            //arrange
            var faker = new Faker();
            var password = faker.Random.String(10);
            var user = new RegisterUserCommand(
                string.Empty,
                faker.Person.LastName,
                faker.Person.UserName,
                faker.Person.Email,
                faker.Person.Phone,
                password,
                password
                );

            var usermanager = Substitute.For<IUserManager>();
            var handler = new RegisterUserCommandHandler(usermanager);
            usermanager.CreateByPasswordAsync(Arg.Any<UserEntity>(), user.Password, CancellationToken.None)
                              .Returns(Task.FromResult(IdentityResult.Success));
            //act
            var userRegisterResult = await handler.Handle(user, CancellationToken.None);

            //assert
            userRegisterResult.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public async Task Creating_User_Should_Be_False_If_We_Pass_Not_Equal_Password_And_RepeatPassword()
        {
            //arrange
            var faker = new Faker();
            var password = faker.Random.String(10);
            var repeatPassword = faker.Random.String(10);
            var user = new RegisterUserCommand(
                string.Empty,
                faker.Person.LastName,
                faker.Person.UserName,
                faker.Person.Email,
                faker.Person.Phone,
                password,
                repeatPassword
                );

            var usermanager = Substitute.For<IUserManager>();
            var handler = new RegisterUserCommandHandler(usermanager);
            usermanager.CreateByPasswordAsync(Arg.Any<UserEntity>(), user.Password, CancellationToken.None)
                              .Returns(Task.FromResult(IdentityResult.Success));
            //act
            var userRegisterResult = await handler.Handle(user, CancellationToken.None);

            //assert
            userRegisterResult.IsSuccess.Should().BeFalse();
        }
    }
}