using Domain.Common;
using Domain.Entities.Ad;
using FluentAssertions;
using static Domain.Entities.Ad.AdEntitiy;

namespace Domain.Tests.AdTests;

public class AddEntityTests
{
    [Fact]
    public void Creating_Ads_With_Empty_User_Should_Throw_Exception()
    {
        //Arrange
        var description = "Test description";
        var title = "Ad name";
        Guid userId = Guid.Empty;
        Guid categoryId = Guid.NewGuid();
        Guid locationid = Guid.NewGuid();

        //Act
        Action act = () => Create(description, title, userId, categoryId,locationid);

        //Asserte
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Creating_Ads_With_Empty_Category_Should_Throw_Exception()
    {
        //Arrange
        var description = "Test description";
        var title = "Ad name";
        Guid userId = Guid.NewGuid(); 
        Guid categoryId = Guid.Empty;
        Guid locationid = Guid.NewGuid();

        //Act
        Action act = () => Create(description, title, userId, categoryId,locationid);

        //Asserte
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Creating_Ads_With_Null_userid_Should_Throw_Exception()
    {
        //Arrange
        var description = "Test description";
        var title = "Ad name";
        Guid? userId = null;
        Guid categoryId = Guid.NewGuid();
        Guid locationid = Guid.Empty;

        //Act
        Action act = () => Create(description, title, userId, categoryId, locationid);

        //Asserte
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Tow_ads_with_the_same_properties_Must_Be_equal()
    {
        //Arrange
        var description = "Test description";
        var title = "Ad name";
        Guid? userId = Guid.NewGuid();
        Guid categoryId = Guid.NewGuid();
        var id = Guid.NewGuid();
        Guid locationid = Guid.NewGuid();

        //Act
        var ad1 = Create(id,title,description,userId,categoryId,locationid);
        var ad2 = Create(id,title, description,userId,categoryId, locationid);

        //Asserte
        ad1.Equals(ad2).Should().BeTrue();
    }

    [Fact]
    public void Creating_And_Ad_Insert_Log()
    {
        //Arrange
        Guid categoryid = Guid.NewGuid();
        var id = Guid.NewGuid();
        var locationid = Guid.NewGuid();
        string title = "new title";
        string description = "new description";
        var userId = Guid.NewGuid();

        //Act
        AdEntitiy ad = Create(id, title, description, userId, categoryid, locationid);

        //Assert

        ad.Logs.Should().HaveCount(1);
    }

    [Fact]
    public void Changing_Ad_State_From_Approved_To_Another_State_Not_Allowed()
    {
        //Arrange
        Guid categoryid = Guid.NewGuid();
        var id = Guid.NewGuid();
        var locationid = Guid.NewGuid();
        string title = "new title";
        string description = "new description";
        var userId = Guid.NewGuid();

        //Act
        AdEntitiy ad = Create(id, title, description, userId, categoryid, locationid);

        //Assert
        ad.ChangeState(AdState.Approved);
        ad.ChangeState(AdState.Pending).Should().Be(new DomainResult(false, "This ad is already approved!"));
    }

    [Fact]
    public void Edit_User_Should_Log_After_Editing_An_Ad()
    {
        //Arrange
        Guid categoryid = Guid.NewGuid();
        var locationid = Guid.NewGuid();
        string title = "new title";
        string description = "new description";
        var userId = Guid.NewGuid();
        //Act
        var ad = Create(title,description,userId,categoryid,locationid);
        
        ad.Edit(title,description,categoryid,locationid);

        ad.Logs.Should().HaveCountGreaterThan(1);
    }
}
