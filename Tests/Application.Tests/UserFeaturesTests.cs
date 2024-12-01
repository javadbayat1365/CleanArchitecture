using Application.Tests.Extensions;
using Application.Contracts.User;
using Application.Features.User.Commands.Register;
using Bogus;
using Domain.Entities.User;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit.Abstractions;
using Xunit.Sdk;
using Application.Features.User.Queries.PasswordLogin;
using Application.Contracts.User.Models;

namespace Application.Tests
{
    public class UserFeaturesTests(ITestOutputHelper testOutputHelper)
    {
        [Fact]
        public async Task Creating_New_User_Should_Be_Success()
        {
            //Arrange
            var faker = new Faker();
            var password = Guid.NewGuid().ToString("N");
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

            testOutputHelper.WriteLineOperationResultErrors(userRegisterResult);
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
            testOutputHelper.WriteLineOperationResultErrors(userRegisterResult);
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
            testOutputHelper.WriteLineOperationResultErrors(userRegisterResult);
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
            testOutputHelper.WriteLineOperationResultErrors(userRegisterResult);
        }
        [Fact]
        public async Task Creating_User_Should_Be_False_If_We_Pass_Not_Equal_Password_And_RepeatPassword()
        {
            //arrange
            var faker = new Faker();
            var password = faker.Random.String(10);
            var repeatPassword = faker.Random.String(10);
            var user = new RegisterUserCommand(
                faker.Person.FirstName,
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
            testOutputHelper.WriteLineOperationResultErrors(userRegisterResult);
        }

        [Fact]
        public async void Creating_User_Should_Be_False_If_PhoneNumber_Is_Not_Number()
        {
            var faker = new Faker();
            var password = Guid.NewGuid().ToString("N");
            var userManager = Substitute.For<IUserManager>();
            var registerUserCommand = new RegisterUserCommand(
                 faker.Person.FirstName,
                 faker.Person.LastName,
                 faker.Person.UserName,
                 faker.Person.Email,
                 "test",
                 password,password);

            await userManager.CreateByPasswordAsync(Arg.Any<UserEntity>(), password, CancellationToken.None);

            var registerUserCommandHandler = new RegisterUserCommandHandler(userManager);

            var result = await registerUserCommandHandler.Handle(registerUserCommand, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();

            testOutputHelper.WriteLineOperationResultErrors(result);
        }

        [Fact]
        public async Task Login_UserName_Should_Be_Success()
        {
            //Arrange
            var faker = new Faker();
            var loginQuery = new UserPasswordLoginQuery(faker.Person.UserName,Guid.NewGuid().ToString("N"));
            var userManager = Substitute.For<IUserManager>();
            var jwtService = Substitute.For<IJwtService>();
            var userEntity = new UserEntity(
                faker.Person.FirstName,
                faker.Person.LastName,
                faker.Person.UserName,
                faker.Person.Email
                )
            { PhoneNumber = "0936" };

            userManager.GetUserByUserNameAsync(loginQuery.UserNameOrEmail,CancellationToken.None)
                             .Returns(Task.FromResult<UserEntity?>(userEntity));

            userManager.CreateByPasswordAsync(userEntity,loginQuery.Password, CancellationToken.None)
                             .Returns(Task.FromResult(IdentityResult.Success));

            jwtService.GenerateTokenAsync(userEntity,CancellationToken.None)
                            .Returns(Task.FromResult(new JwtAccessTokenModel("AccessToken",3000)));

            //Act
            var userLoginQueryHandler = new UserPasswordLoginQueryHandler(userManager,jwtService);

            var loginResult =await userLoginQueryHandler.Handle(loginQuery,CancellationToken.None);

            loginResult.Result.Should().NotBeNull();
            loginResult.IsSuccess.Should().BeTrue();

        }

        [Fact]
        public async Task Login_User_Should_Be_Fail_With_Wronge_Password()
        {
            //Arrange
            var faker = new Faker();
            var loginQuery = new UserPasswordLoginQuery(faker.Person.UserName, Guid.NewGuid().ToString("N"));
            var userManager = Substitute.For<IUserManager>();
            var jwtService = Substitute.For<IJwtService>();
            var userEntity = new UserEntity(
                faker.Person.FirstName,
                faker.Person.LastName,
                faker.Person.UserName,
                faker.Person.Email
                )
            { PhoneNumber = "0936" };

            userManager.GetUserByUserNameAsync(loginQuery.UserNameOrEmail, CancellationToken.None)
                             .Returns(Task.FromResult<UserEntity?>(userEntity));

            userManager.CreateByPasswordAsync(userEntity, loginQuery.Password, CancellationToken.None)
                             .Returns(Task.FromResult(IdentityResult.Failed()));

            jwtService.GenerateTokenAsync(userEntity, CancellationToken.None)
                            .Returns(Task.FromResult(new JwtAccessTokenModel("AccessToken", 3000)));

            //Act
            var userLoginQueryHandler = new UserPasswordLoginQueryHandler(userManager, jwtService);

            var loginResult = await userLoginQueryHandler.Handle(loginQuery, CancellationToken.None);

            loginResult.Result.Should().BeNull();
            loginResult.IsSuccess.Should().BeFalse();
            testOutputHelper.WriteLineOperationResultErrors(loginResult);
        }

        [Fact]
        public async Task Login_With_Email_Should_Be_Success() 
        {
            //Arrange
            var faker = new Faker();
            var loginQuery = new UserPasswordLoginQuery(faker.Person.Email, Guid.NewGuid().ToString("N"));
            var userManager = Substitute.For<IUserManager>();
            var jwtService = Substitute.For<IJwtService>();
            var userEntity = new UserEntity(
                faker.Person.FirstName,
                faker.Person.LastName,
                faker.Person.UserName,
                faker.Person.Email
                )
            { PhoneNumber = "0936" };

            userManager.GetUserByEmailAsync(loginQuery.UserNameOrEmail, CancellationToken.None)
                             .Returns(Task.FromResult<UserEntity?>(userEntity));

            userManager.CreateByPasswordAsync(userEntity, loginQuery.Password, CancellationToken.None)
                             .Returns(Task.FromResult(IdentityResult.Success));

            jwtService.GenerateTokenAsync(userEntity, CancellationToken.None)
                            .Returns(Task.FromResult(new JwtAccessTokenModel("AccessToken", 3000)));

            //Act
            var userLoginQueryHandler = new UserPasswordLoginQueryHandler(userManager, jwtService);

            var loginResult = await userLoginQueryHandler.Handle(loginQuery, CancellationToken.None);

            loginResult.Result.Should().NotBeNull();
            loginResult.IsSuccess.Should().BeTrue();

        }

        [Fact]
        public async Task Login_With_Null_User_Should_Be_Faile()
        {
            //Arrange
            var faker = new Faker();
            var loginQuery = new UserPasswordLoginQuery(faker.Person.Email, Guid.NewGuid().ToString("N"));
            var userManager = Substitute.For<IUserManager>();
            var jwtService = Substitute.For<IJwtService>();
            var userEntity = new UserEntity(
                faker.Person.FirstName,
                faker.Person.LastName,
                faker.Person.UserName,
                faker.Person.Email
                )
            { PhoneNumber = "0936" };

            userManager.GetUserByEmailAsync(loginQuery.UserNameOrEmail, CancellationToken.None)
                             .Returns(Task.FromResult<UserEntity?>(null));

            userManager.CreateByPasswordAsync(userEntity, loginQuery.Password, CancellationToken.None)
                             .Returns(Task.FromResult(IdentityResult.Success));

            jwtService.GenerateTokenAsync(userEntity, CancellationToken.None)
                            .Returns(Task.FromResult(new JwtAccessTokenModel("AccessToken", 3000)));

            //Act
            var userLoginQueryHandler = new UserPasswordLoginQueryHandler(userManager, jwtService);

            var loginResult = await userLoginQueryHandler.Handle(loginQuery, CancellationToken.None);

            loginResult.Result.Should().BeNull();
            loginResult.IsNotFound.Should().BeTrue();
            testOutputHelper.WriteLineOperationResultErrors(loginResult);
        }
    }
}