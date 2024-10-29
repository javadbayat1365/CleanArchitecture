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
        Action act = () => AdEntitiy.Create(description, title, userId, categoryId,locationid);

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
        Action act = () => AdEntitiy.Create(description, title, userId, categoryId,locationid);

        //Asserte
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Creating_Ads_With_Null_Category_Should_Throw_Exception()
    {
        //Arrange
        var description = "Test description";
        var title = "Ad name";
        Guid? userId = Guid.NewGuid();
        Guid categoryId = Guid.Empty;
        Guid locationid = Guid.NewGuid();

        //Act
        Action act = () => AdEntitiy.Create(description, title, userId, categoryId, locationid);

        //Asserte
        act.Should().Throw<ArgumentNullException>();
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
        Action act = () => AdEntitiy.Create(description, title, userId, categoryId, locationid);

        //Asserte
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Tow_ads_with_the_same_category_Must_Be_equal()
    {
        //Arrange
        var description = "Test description";
        var title = "Ad name";
        Guid? userId = Guid.NewGuid();
        Guid categoryId = Guid.NewGuid();
        var id = Guid.NewGuid();
        Guid locationid = Guid.NewGuid();

        //Act
        var ad1 = AdEntitiy.Create(id,title,description,userId,categoryId,locationid);
        var ad2 = AdEntitiy.Create(id,title, description,userId,categoryId, locationid);

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
        AdEntitiy ad = AdEntitiy.Create(id, title, description, userId, categoryid, locationid);

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
        AdEntitiy ad = AdEntitiy.Create(id, title, description, userId, categoryid, locationid);

        //Assert
        ad.ChangeState(AdState.Approved);

        ad.ChangeState(AdState.Pending).Should().Be(new DomainResult(false, "Ad State Changed!"));
    }

    [Fact]
    public void Should_Log_After_Editing_An_Ad()
    {
        //Arrange
        Guid categoryid = Guid.NewGuid();
        var locationid = Guid.NewGuid();
        string title = "new title";
        string description = "new description";
        var userId = Guid.NewGuid();
        //Act
        var ad = AdEntitiy.Create(title,description,userId,categoryid,locationid);
        
        ad.Edit(title,description,categoryid,locationid);

        ad.Logs.Should().HaveCountGreaterThan(1);
    }
}
