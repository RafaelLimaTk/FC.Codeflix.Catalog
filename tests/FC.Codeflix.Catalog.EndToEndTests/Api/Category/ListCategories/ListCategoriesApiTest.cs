using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using FC.Codeflix.Catalog.Domain.SeedWorks.SearchableRepository;
using FC.Codeflix.Catalog.EndToEndTests.Extensions.DateTime;
using FC.Codeflix.Catalog.EndToEndTests.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.ListCategories;

[Collection(nameof(ListCategoriesApiTestFixture))]
public class ListCategoriesApiTest
    : IDisposable
{
    private readonly ListCategoriesApiTestFixture _fixture;

    public ListCategoriesApiTest(ListCategoriesApiTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(ListCategoriesAndTotalByDefault))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    public async Task ListCategoriesAndTotalByDefault()
    {
        var defaultPerPage = 15;
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var (httpMessage, response) = await _fixture.ApiClient
            .Get<TestApiResponseList<CategoryModelResponse>>("/categories");

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Data.Should().NotBeNull();
        response.Data.Should().HaveCount(defaultPerPage);
        response.Meta.Total.Should().Be(exampleCategoriesList.Count);
        foreach (CategoryModelResponse outputItem in response.Data)
        {
            var exampleItem = exampleCategoriesList
                .FirstOrDefault(x => x.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
        }
    }

    [Fact(DisplayName = nameof(ItemsEmptyWhenPersistenceEmpty))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    public async Task ItemsEmptyWhenPersistenceEmpty()
    {
        var (httpMessage, response) = await _fixture.ApiClient
            .Get<TestApiResponseList<CategoryModelResponse>>("/categories");

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Data.Should().NotBeNull();
        response.Data.Should().HaveCount(0);
        response.Meta.Total.Should().Be(0);
    }

    [Fact(DisplayName = nameof(ListCategoriesAndTotal))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    public async Task ListCategoriesAndTotal()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var input = new ListCategoriesRequest(page: 1, perPage: 5);

        var (httpMessage, response) = await _fixture.ApiClient
            .Get<TestApiResponseList<CategoryModelResponse>>("/categories", input);

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Data.Should().NotBeNull();
        response.Data.Should().HaveCount(input.PerPage);
        response.Meta.Total.Should().Be(exampleCategoriesList.Count);
        response.Meta.CurrentPage.Should().Be(input.Page);
        response.Meta.PerPage.Should().Be(input.PerPage);
        foreach (CategoryModelResponse outputItem in response.Data)
        {
            var exampleItem = exampleCategoriesList
                .FirstOrDefault(x => x.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
        }
    }

    [Theory(DisplayName = nameof(ListPaginated))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task ListPaginated(
        int quantityCategoriesToGenerate,
        int page,
        int perPage,
        int expectedQuantityItems
    )
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(
            quantityCategoriesToGenerate
        );
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var input = new ListCategoriesRequest(page, perPage);

        var (httpMessage, response) = await _fixture.ApiClient
            .Get<TestApiResponseList<CategoryModelResponse>>("/categories", input);

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Data.Should().NotBeNull();
        response.Data.Should().HaveCount(expectedQuantityItems);
        response.Meta.Total.Should().Be(exampleCategoriesList.Count);
        response.Meta.CurrentPage.Should().Be(input.Page);
        response.Meta.PerPage.Should().Be(input.PerPage);
        foreach (CategoryModelResponse outputItem in response.Data)
        {
            var exampleItem = exampleCategoriesList.Find(
                category => category.Id == outputItem.Id
            );
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.TrimMillisseconds().Should().Be(
                exampleItem.CreatedAt.TrimMillisseconds()
            );
        }
    }

    [Theory(DisplayName = nameof(SearchByText))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Sci-fi Other", 1, 3, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchByText(
        string search,
        int page,
        int perPage,
        int expectedQuantityItems,
        int expectedTotal
    )
    {
        var categoryNamesList = new List<string>() {
        "Action",
        "Horror",
        "Horror - Robots",
        "Horror - Based on Real Facts",
        "Drama",
        "Sci-fi IA",
        "Sci-fi Space",
        "Sci-fi Robots",
        "Sci-fi Future"
    };

        var exampleCategoriesList = _fixture
            .GetExampleCategoriesListWithNames(categoryNamesList);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var input = new ListCategoriesRequest(page, perPage, search: search);

        var (httpMessage, response) = await _fixture.ApiClient
            .Get<TestApiResponseList<CategoryModelResponse>>("/categories", input);

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Data.Should().NotBeNull();
        response.Data.Should().HaveCount(expectedQuantityItems);
        response.Meta.Total.Should().Be(expectedTotal);
        response.Meta.CurrentPage.Should().Be(input.Page);
        response.Meta.PerPage.Should().Be(input.PerPage);
        foreach (CategoryModelResponse outputItem in response.Data)
        {
            var exampleItem = exampleCategoriesList.Find(
                category => category.Id == outputItem.Id
            );
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.TrimMillisseconds().Should().Be(
                exampleItem.CreatedAt.TrimMillisseconds()
            );
        }
    }

    [Theory(DisplayName = nameof(ListOrdered))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("", "asc")]
    public async Task ListOrdered(
        string orderBy,
        string order
    )
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(10);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var inputOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var input = new ListCategoriesRequest(
            page: 1,
            perPage: 20,
            sort: orderBy,
            dir: inputOrder
        );

        var (httpMessage, response) = await _fixture.ApiClient
            .Get<TestApiResponseList<CategoryModelResponse>>("/categories", input);

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Data.Should().NotBeNull();
        response.Data.Should().HaveCount(exampleCategoriesList.Count);
        response.Meta.Total.Should().Be(exampleCategoriesList.Count);
        var orderedList = _fixture.CloneCategoriesListOrdered(
            exampleCategoriesList,
            input.Sort,
            input.Dir
        );

        for (var i = 0; i < response.Data.Count; i++)
        {
            response.Data[i].Id.Should().Be(orderedList[i].Id);
            response.Data[i].Name.Should().Be(orderedList[i].Name);
            response.Data[i].Description.Should().Be(orderedList[i].Description);
            response.Data[i].IsActive.Should().Be(orderedList[i].IsActive);
            response.Data[i].CreatedAt.TrimMillisseconds().Should().Be(
                orderedList[i].CreatedAt.TrimMillisseconds()
            );
        }
    }

    public void Dispose()
        => _fixture.CleanPersistence();
}
