using Domain.Entities.Ad;
using FluentAssertions;
using Persistence.Repositories.Common;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Persistance.Test;

public class UnitOfWorkTests:IClassFixture<PersistenceTestSetup>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly ITestOutputHelper  _testOutputHelper;
    public UnitOfWorkTests(PersistenceTestSetup setup, ITestOutputHelper testOutputHelper)
    {
        _unitOfWork = setup.UnitOfWork;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task Adding_New_Location_Should_Save_To_Database()
    {
        var location = new LocationEntity("Test Location");

        await _unitOfWork.LocationRepository.CreateAsync(location);
        await _unitOfWork.CommitAsync();
        var locationInDb = await _unitOfWork.LocationRepository.GetLocationsByNameAsync(location.Name);

        locationInDb.Should().NotBeNull();
    }

    [Fact]
    public async Task Getting_Location_By_Id_Should_Success()
    {
        var location = new LocationEntity("Test Location 2");
        await _unitOfWork.LocationRepository.CreateAsync(location);
        await _unitOfWork.CommitAsync();

        var locationById = await _unitOfWork.LocationRepository.GetLocationByIdAsync(location.Id);

        locationById.Should().NotBeNull();
    }

    [Fact]
    public async Task Location_Added_Date_Should_Have_Valid()
    {
        var location = new LocationEntity("Test Location 3");
        await _unitOfWork.LocationRepository.CreateAsync(location);
        await _unitOfWork.CommitAsync();

        var locationById = await _unitOfWork.LocationRepository.GetLocationByIdAsync(location.Id);

        locationById.CreateDate.Should().BeMoreThan(TimeSpan.MinValue);

        _testOutputHelper.WriteLine("Current Added Location Date: "+locationById.CreateDate);
    }

    [Fact]
    public async Task Location_Modified_Added_Date_Should_Not_Have_Value_When_Added()
    {
        var location = new LocationEntity("Test Location 4");
        await _unitOfWork.LocationRepository.CreateAsync(location);
        await _unitOfWork.CommitAsync();

        var locationById = await _unitOfWork.LocationRepository.GetLocationByIdAsync(location.Id);

        locationById.ModifiedDate.Should().BeNull();

        _testOutputHelper.WriteLine("Current Added Location ModifiedDate Date: " + locationById.ModifiedDate);
    }

    [Fact]
    public async Task Location_Modified_Date_Should_Have_Value_When_Edit_Name()
    {
        var location = new LocationEntity("Test Location 4");
        await _unitOfWork.LocationRepository.CreateAsync(location);
        await _unitOfWork.CommitAsync();

        var locationById = await _unitOfWork.LocationRepository.GetLocationByIdForEditAsync(location.Id);
        locationById.EditName("Test Location New Name");

        await _unitOfWork.CommitAsync();

        var locationByIdNewName = await _unitOfWork.LocationRepository.GetLocationByIdAsync(location.Id);

        locationById.ModifiedDate.Should().BeMoreThan(TimeSpan.MinValue);

        _testOutputHelper.WriteLine("Current Added Location ModifiedDate Date: " + locationByIdNewName.ModifiedDate);
    }
}
