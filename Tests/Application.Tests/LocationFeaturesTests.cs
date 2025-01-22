using Application.Common;
using Application.Extensions;
using Application.Features.Location.Commands;
using Application.Features.Location.Queries.GetLocationsByName;
using Application.Repositories.Common;
using Application.Repositories.Location;
using Application.Tests.Extensions;
using Bogus;
using Domain.Entities.Ad;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit.Abstractions;

namespace Application.Tests;

public class LocationFeaturesTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITestOutputHelper _testOutputHelper;

    public LocationFeaturesTests(ITestOutputHelper testOutputHelper)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.RegisterApplicationValidators();
        _serviceProvider = serviceCollection.BuildServiceProvider();
        _testOutputHelper = testOutputHelper;
    }
    [Fact]
    public async Task Add_Location_With_Valid_Parameters_should_Success()
    {
        //Arrange
        var faker = new Faker();
        
        var location = new CreateLocationCommand(faker.Address.City());
        var locationRepositoryMock = Substitute.For<ILocationRepository>();
        var unitOfWorkMock = Substitute.For<IUnitOfWork>();

        locationRepositoryMock.IsLocationExistAsync(location.LocationName)
            .Returns(Task.FromResult(false));

        unitOfWorkMock.LocationRepository.Returns(locationRepositoryMock);

        var validationBehavior = new ValidateRequestBehavior<CreateLocationCommand, OperationResult<bool>>
                (_serviceProvider.GetRequiredService<IValidator<CreateLocationCommand>>());

        var createLocationHandler = new CreateLocationCommandHandler(unitOfWorkMock);

        //Act
        var createLocationResult = await validationBehavior.Handle(location,CancellationToken.None,createLocationHandler.Handle);

        //Assert

        createLocationResult.Result.Should().BeTrue();
    }
    [Fact]
    public async Task Existing_Location_Cannot_Be_Created()
    {
        //Arrange
        var faker = new Faker();

        var location = new CreateLocationCommand(faker.Address.City());
        var locationRepositoryMock = Substitute.For<ILocationRepository>();
        var unitOfWorkMock = Substitute.For<IUnitOfWork>();

        locationRepositoryMock.IsLocationExistAsync(location.LocationName)
            .Returns(Task.FromResult(true));

        unitOfWorkMock.LocationRepository.Returns(locationRepositoryMock);

        var validationBehavior = new ValidateRequestBehavior<CreateLocationCommand, OperationResult<bool>>
                (_serviceProvider.GetRequiredService<IValidator<CreateLocationCommand>>());

        var createLocationHandler = new CreateLocationCommandHandler(unitOfWorkMock);

        //Act
        var createLocationResult = await validationBehavior.Handle(location, CancellationToken.None, createLocationHandler.Handle);

        //Assert

        createLocationResult.Result.Should().BeFalse();
        _testOutputHelper.WriteLineOperationResultErrors(createLocationResult);
    }

    [Fact]
    public async Task  Getting_List_Of_Locations_Should_Be_Success()
    {
        //Arrange
        var faker = new Faker();

        var location = new GetLocationsByNameQuery(faker.Address.City());
        var locationRepositoryMock = Substitute.For<ILocationRepository>();
        var unitOfWorkMock = Substitute.For<IUnitOfWork>();
        List<LocationEntity> locations = 
            [
            new LocationEntity(faker.Address.City()),
            new LocationEntity(faker.Address.City()),
            new LocationEntity(faker.Address.City()),
            ];
        locationRepositoryMock.GetLocationsByNameAsync(location.LocationName)
            .Returns(Task.FromResult(locations));

        unitOfWorkMock.LocationRepository.Returns(locationRepositoryMock);

        var validationBehavior = new ValidateRequestBehavior<GetLocationsByNameQuery,OperationResult<IEnumerable<GetLocationsByNameQueryResult>>>
                (_serviceProvider.GetRequiredService<IValidator<GetLocationsByNameQuery>>());

        var getLocationsByNameHandler = new GetLocationsByNameQueryHandler(unitOfWorkMock);

        //Act
        var createLocationResult = await validationBehavior.Handle(location, CancellationToken.None, getLocationsByNameHandler.Handle);

        //Assert

        createLocationResult.Result.Should().NotBeEmpty();
        createLocationResult.Result.Should().HaveCountGreaterThan(2);
        _testOutputHelper.WriteLineOperationResultErrors(createLocationResult);
    }
    [Fact]
    public async Task Searching_For_Location_Should_Return_False_When_Leas_Than_Three_Characters_Sent()
    {
        //Arrange
        var faker = new Faker();

        var location = new GetLocationsByNameQuery(faker.Address.City()[..2]);
        var locationRepositoryMock = Substitute.For<ILocationRepository>();
        var unitOfWorkMock = Substitute.For<IUnitOfWork>();
        List<LocationEntity> locations =
            [
            new LocationEntity(faker.Address.City()),
            new LocationEntity(faker.Address.City()),
            new LocationEntity(faker.Address.City()),
            ];
        locationRepositoryMock.GetLocationsByNameAsync(location.LocationName)
            .Returns(Task.FromResult(locations));

        unitOfWorkMock.LocationRepository.Returns(locationRepositoryMock);

        var validationBehavior = new ValidateRequestBehavior<GetLocationsByNameQuery, OperationResult<IEnumerable<GetLocationsByNameQueryResult>>>
                (_serviceProvider.GetRequiredService<IValidator<GetLocationsByNameQuery>>());

        var getLocationsByNameHandler = new GetLocationsByNameQueryHandler(unitOfWorkMock);

        //Act
        var getLocationResult = await validationBehavior.Handle(location, CancellationToken.None, getLocationsByNameHandler.Handle);

        //Assert

        getLocationResult.IsSuccess.Should().BeFalse();

        _testOutputHelper.WriteLineOperationResultErrors(getLocationResult);
    }
    [Fact]
    public async Task Searching_For_Location_Should_Return_True_When_More_Than_Three_Characters_Sent()
    {
        //Arrange
        var faker = new Faker();

        var location = new GetLocationsByNameQuery(faker.Address.City()[..3]);
        var locationRepositoryMock = Substitute.For<ILocationRepository>();
        var unitOfWorkMock = Substitute.For<IUnitOfWork>();
        List<LocationEntity> locations =
            [
            new LocationEntity(faker.Address.City()),
            new LocationEntity(faker.Address.City()),
            new LocationEntity(faker.Address.City()),
            ];
        locationRepositoryMock.GetLocationsByNameAsync(location.LocationName)
            .Returns(Task.FromResult(locations));

        unitOfWorkMock.LocationRepository.Returns(locationRepositoryMock);

        var validationBehavior = new ValidateRequestBehavior<GetLocationsByNameQuery, OperationResult<IEnumerable<GetLocationsByNameQueryResult>>>
                (_serviceProvider.GetRequiredService<IValidator<GetLocationsByNameQuery>>());

        var getLocationsByNameHandler = new GetLocationsByNameQueryHandler(unitOfWorkMock);

        //Act
        var getLocationResult = await validationBehavior.Handle(location, CancellationToken.None, getLocationsByNameHandler.Handle);

        //Assert

        getLocationResult.IsSuccess.Should().BeTrue();

        _testOutputHelper.WriteLineOperationResultErrors(getLocationResult);
    }
}
