using Application.Contracts.FileService.Interfaces;
using Application.Contracts.FileService.Models;
using Application.Contracts.User;
using Application.Extensions;
using Application.Features.Ad.Commands;
using Application.Features.Ad.Commands.CreateAd;
using Application.Features.Ad.Commands.EditAd;
using Application.Repositories.Ad;
using Application.Repositories.Category;
using Application.Repositories.Common;
using Application.Repositories.Location;
using Application.Tests.Extensions;
using Domain.Common.ValueObjects;
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

    [Fact]
    public async Task Editing_An_Ad_With_valid_Parameters_Should_Be_Success()
    {
        //Arrange
        var mockId = Guid.NewGuid();
        var adEntityMock =AdEntitiy.Create(mockId,"test Title", "Test Description",Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        var mockAdImages = new List<ImageValueObject>() { 
         new ImageValueObject("TestFile1.png","image/png"),
         new ImageValueObject("TestFile2.png","image/png"),
         new ImageValueObject("TestFile3.png","image/png")
        };

        mockAdImages.ForEach(c => adEntityMock.AddImage(c));

        var unitOfWorkMock = Substitute.For<IUnitOfWork>();
        var adRepositoryMock = Substitute.For<IAdRepository>();
        var fileServiceMock = Substitute.For<IFileService>();
        var locationRepositoryMock = Substitute.For<ILocationRepository>();
        var categoryRepositoryMock = Substitute.For<ICategoryRepository>();


        locationRepositoryMock.GetLocationByIdAsync(Arg.Any<Guid>()).Returns(Task.FromResult<LocationEntity?>(new LocationEntity("location")));
        categoryRepositoryMock.GetCategoryByIdAsync(Arg.Any<Guid>()).Returns(Task.FromResult<CategoryEntity?>(new CategoryEntity("category")));

        adRepositoryMock.GetAdByIdForUpdateAsync(mockId,CancellationToken.None)!.Returns(Task.FromResult(adEntityMock));

        fileServiceMock.SaveFilesAsync(Arg.Any<List<SaveFileModel>>()).Returns(Task.FromResult(new List<SaveFileModelResult>() {
         new("Test4.png","image/png")
        }));


        fileServiceMock.RemoveFileAsync(Arg.Any<string[]>()).Returns(Task.CompletedTask);

        unitOfWorkMock.AdRepository.Returns(adRepositoryMock);
        unitOfWorkMock.LocationRepository.Returns(locationRepositoryMock);
        unitOfWorkMock.CategoryRepository.Returns(categoryRepositoryMock);

        var editAdCommand = new EditAdCommand(
            mockId,
            "Edited Title",
            "Edited Description",
            Guid.NewGuid(),
            Guid.NewGuid(),
            ["TestFile1.png"],
            [new EditAdCommand.AddNewImagesModel("TestFileAdd.png","image/png")]
            );

        var editAdResultHandler = new EditAdCommandHandler(unitOfWorkMock, fileServiceMock);

        var editAdResult = await Helpers.ValidateAndExcuteAsync(editAdCommand, editAdResultHandler, _serviceProvider);
        //Act



        //Assert
        editAdResult.Result.Should().BeTrue();
        adEntityMock.Title.Should().BeEquivalentTo("Edited Title");
        adEntityMock.Description.Should().BeEquivalentTo("Edited Description");
        adEntityMock.Images.Should().NotContain(c => c.FileName.Equals("TestFile1.png"));
        adEntityMock.Images.Should().HaveCount(3);
        adEntityMock.Images.Should().Contain(c => c.FileName.Contains("Test4.png"));
        adEntityMock.CurrentState.Should().Be(AdEntitiy.AdState.Pending);
        _testOutputHelper.WriteLineOperationResultErrors(editAdResult);
    }
}
