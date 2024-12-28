using Application.Extensions;
using Application.Features.Category.Queries.GetCategoryById;
using Application.Repositories.Category;
using Application.Repositories.Common;
using Application.Tests.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit.Abstractions;

namespace Application.Tests;

public class CategoryFeaturesTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITestOutputHelper _testOutputHelper;

    public CategoryFeaturesTests(ITestOutputHelper testOutputHelper)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.RegisterApplicationValidators();
        _serviceProvider = serviceCollection.BuildServiceProvider();
        _testOutputHelper = testOutputHelper;
    }
    [Fact]
    public async Task Get_Category_By_Id_Should_Return_Success()
    {
        //Arrange
        var getCategoryByIdQuery = new GetCategoryByIdQuery(Guid.NewGuid());
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var categoryReposiotry = Substitute.For<ICategoryRepository>();

        categoryReposiotry.GetCategoryByIdAsync(getCategoryByIdQuery.CategoryId)!
            .Returns(Task.FromResult(new Domain.Entities.Ad.CategoryEntity("Test")));

        unitOfWork.CategoryRepository.Returns(categoryReposiotry);

        var handler = new GetCategoryByIdQueryHandler(unitOfWork);

        //Act
        var result = await Helpers.ValidateAndExcuteAsync(getCategoryByIdQuery,handler,_serviceProvider);

        //Assert
        result.Result.Should().NotBeNull();
    }

    [Fact]
    public async Task Get_Category_By_Empty_Id_Should_Not_Return_Success()
    {
        //Arrange
        var getCategoryByIdQuery = new GetCategoryByIdQuery(Guid.Empty);
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var categoryReposiotry = Substitute.For<ICategoryRepository>();

        categoryReposiotry.GetCategoryByIdAsync(getCategoryByIdQuery.CategoryId)!
            .Returns(Task.FromResult(new Domain.Entities.Ad.CategoryEntity("Test")));

        unitOfWork.CategoryRepository.Returns(categoryReposiotry);

        var handler = new GetCategoryByIdQueryHandler(unitOfWork);

        //Act
        var result = await Helpers.ValidateAndExcuteAsync(getCategoryByIdQuery, handler, _serviceProvider);

        //Assert
        result.Result.Should().BeNull();

        _testOutputHelper.WriteLineOperationResultErrors(result);
    }
}
