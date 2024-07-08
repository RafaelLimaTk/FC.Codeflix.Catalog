using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
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
            .Get<ListCategoriesResponse>("/categories");

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Total.Should().Be(exampleCategoriesList.Count);
        response!.Items.Should().HaveCount(defaultPerPage);
        foreach (CategoryModelResponse outputItem in response.Items)
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
            .Get<ListCategoriesResponse>("/categories");

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Items.Should().NotBeNull();
        response!.Total.Should().Be(0);
        response.Items.Should().HaveCount(0);
    }

    [Fact(DisplayName = nameof(ListCategoriesAndTotal))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    public async Task ListCategoriesAndTotal()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var input = new ListCategoriesRequest(page: 1, perPage: 5);

        var (httpMessage, response) = await _fixture.ApiClient
            .Get<ListCategoriesResponse>("/categories", input);

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Total.Should().Be(exampleCategoriesList.Count);
        response.Page.Should().Be(input.Page);
        response.PerPage.Should().Be(input.PerPage);
        response!.Items.Should().HaveCount(input.PerPage);
        foreach (CategoryModelResponse outputItem in response.Items)
        {
            var exampleItem = exampleCategoriesList
                .FirstOrDefault(x => x.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
        }
    }

    public void Dispose()
        => _fixture.CleanPersistence();
}
