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
        var name = "Ad name";
        Guid userId = Guid.Empty;
        Guid categoryId = Guid.NewGuid();

        //Act
        Action act = () => AdEntitiy.Create(description, name, userId, categoryId);

        //Asserte
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Creating_Ads_With_Empty_Category_Should_Throw_Exception()
    {
        //Arrange
        var description = "Test description";
        var name = "Ad name";
        Guid userId = Guid.NewGuid(); 
        Guid categoryId = Guid.Empty;

        //Act
        Action act = () => AdEntitiy.Create(description, name, userId, categoryId);

        //Asserte
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Creating_Ads_With_Null_Category_Should_Throw_Exception()
    {
        //Arrange
        var description = "Test description";
        var name = "Ad name";
        Guid? userId = Guid.NewGuid();
        Guid? categoryId = null;

        //Act
        Action act = () => AdEntitiy.Create(description, name, userId, categoryId);

        //Asserte
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Creating_Ads_With_Null_userid_Should_Throw_Exception()
    {
        //Arrange
        var description = "Test description";
        var name = "Ad name";
        Guid? userId = null;
        Guid? categoryId = Guid.NewGuid();

        //Act
        Action act = () => AdEntitiy.Create(description, name, userId, categoryId);

        //Asserte
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Tow_ads_with_the_same_category_Must_Be_equal()
    {
        //Arrange
        var description = "Test description";
        var name = "Ad name";
        Guid? userId = Guid.NewGuid();
        Guid? categoryId = Guid.NewGuid();
        var id = Guid.NewGuid();
        //Act
        var ad1 = AdEntitiy.Create(id,name,description,userId,categoryId);
        var ad2 = AdEntitiy.Create(id,name, description,userId,categoryId);

        //Asserte
        ad1.Equals(ad2).Should().BeTrue();
    }
}
