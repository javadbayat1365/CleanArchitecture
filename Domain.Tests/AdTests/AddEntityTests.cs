using Domain.Common;
using Domain.Entities.Ad;
using FluentAssertions;

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
        Guid locationid = Guid.Empty;
        //Act
        Action act = () => AdEntitiy.Create(description, title, userId, categoryId,locationid);

        //Asserte
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Creating_Ads_With_Empty_Category_Should_Throw_Exception()
    {
        //Arrange
        var description = "Test description";
        var title = "Ad name";
        Guid userId = Guid.NewGuid(); 
        Guid categoryId = Guid.Empty;
        Guid locationid = Guid.Empty;

        //Act
        Action act = () => AdEntitiy.Create(description, title, userId, categoryId,locationid);

        //Asserte
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Creating_Ads_With_Null_Category_Should_Throw_Exception()
    {
        //Arrange
        var description = "Test description";
        var title = "Ad name";
        Guid? userId = Guid.NewGuid();
        Guid? categoryId = null;
        Guid locationid = Guid.Empty;

        //Act
        Action act = () => AdEntitiy.Create(description, title, userId, categoryId, locationid);

        //Asserte
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Creating_Ads_With_Null_userid_Should_Throw_Exception()
    {
        //Arrange
        var description = "Test description";
        var title = "Ad name";
        Guid? userId = null;
        Guid? categoryId = Guid.NewGuid();
        Guid locationid = Guid.Empty;

        //Act
        Action act = () => AdEntitiy.Create(description, title, userId, categoryId, locationid);

        //Asserte
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Tow_ads_with_the_same_category_Must_Be_equal()
    {
        //Arrange
        var description = "Test description";
        var title = "Ad name";
        Guid? userId = Guid.NewGuid();
        Guid? categoryId = Guid.NewGuid();
        var id = Guid.NewGuid();
        Guid locationid = Guid.Empty;

        //Act
        var ad1 = AdEntitiy.Create(id,title,description,userId,categoryId,locationid);
        var ad2 = AdEntitiy.Create(id,title, description,userId,categoryId, locationid);

        //Asserte
        ad1.Equals(ad2).Should().BeTrue();
    }
}
