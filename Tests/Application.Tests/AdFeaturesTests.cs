using Application.Contracts.FileService.Interfaces;
using Application.Contracts.FileService.Models;
using Application.Contracts.User;
using Application.Extensions;
using Application.Features.Ad.Commands;
using Application.Repositories.Ad;
using Application.Repositories.Category;
using Application.Repositories.Common;
using Application.Repositories.Location;
using Application.Tests.Extensions;
using Domain.Entities.Ad;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit.Abstractions;

namespace Application.Tests;

public class AdFeaturesTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IServiceProvider _serviceProvider;
    public AdFeaturesTests(ITestOutputHelper testOutputHelper)
    {
        this._testOutputHelper = testOutputHelper;

        var serviceCollection = new ServiceCollection();
        serviceCollection.RegisterApplicationValidators();
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    [Fact]
    public async Task Create_Ad_With_Valid_Parameters_Should_Success()
    {
        //Arrange
        var createAdCommand = new CreateAdCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),"test Title","Test Description",
            new[] { new CreateAdCommand.CreateAdImagesModel("Test", "image/png") });

        var unitOfWorkMock = Substitute.For<IUnitOfWork>();
        //var adRepositoryMock = Substitute.For<IAdRepository>();
        var userManagerMock = Substitute.For<IUserManager>();
        var fileServiceMock = Substitute.For<IFileService>();
        var locationRepositoryMock = Substitute.For<ILocationRepository>();
        var categoryRepositoryMock = Substitute.For<ICategoryRepository>();


        locationRepositoryMock.GetLocationByIdAsync(Arg.Any<Guid>()).Returns(Task.FromResult<LocationEntity?>(new LocationEntity("location")));
        categoryRepositoryMock.GetCategoryByIdAsync(Arg.Any<Guid>()).Returns(Task.FromResult<CategoryEntity?>(new CategoryEntity("category")));

        //adRepositoryMock.CreateAdAsync(Arg.Any<AdEntitiy>())
        //    .Returns(Task.CompletedTask);

        userManagerMock.GetUserByIdAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(new Domain.Entities.User.UserEntity("test", "test", "test", "test@test.com")));

        fileServiceMock.SaveFilesAsync(Arg.Any<List<SaveFileModel>>()).Returns(Task.FromResult(new List<SaveFileModelResult>() { 
         new("Test.png","image/png")
        }));


        //unitOfWorkMock.AdRepository.Returns(adRepositoryMock);
        unitOfWorkMock.LocationRepository.Returns(locationRepositoryMock);
        unitOfWorkMock.CategoryRepository.Returns(categoryRepositoryMock);

        var createAdResultHandler = new CreateAdCommandHandler(unitOfWorkMock,userManagerMock,fileServiceMock);

        var createAdResult = await Helpers.ValidateAndExcuteAsync(createAdCommand,createAdResultHandler,_serviceProvider);
        //Act



        //Assert
        _testOutputHelper.WriteLineOperationResultErrors(createAdResult);
        createAdResult.Result.Should().BeTrue();
    }
}
