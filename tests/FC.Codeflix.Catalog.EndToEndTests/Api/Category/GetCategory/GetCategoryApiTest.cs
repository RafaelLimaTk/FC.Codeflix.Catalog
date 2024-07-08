using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.GetCategory;

[Collection(nameof(GetCategoryApiTestFixture))]
public class GetCategoryApiTest
{
    private readonly GetCategoryApiTestFixture _fixture;

    public GetCategoryApiTest(GetCategoryApiTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(GetCategory))]
    [Trait("EndToEnd/API", "Category/Get - Endpoints")]
    public async Task GetCategory()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var exampleCategory = exampleCategoriesList[10];

        var (httpMessage, response) = await _fixture.ApiClient
            .Get<CategoryModelResponse>(
                $"/categories/{exampleCategory.Id}"
            );

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Should().NotBeNull();
        response!.Id.Should().Be(exampleCategory.Id);
        response.Name.Should().Be(exampleCategory.Name);
        response.Description.Should().Be(exampleCategory.Description);
        response.IsActive.Should().Be(exampleCategory.IsActive);
        response.CreatedAt.Should().BeSameDateAs(exampleCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(ErrorWhenCategoryNotFound))]
    [Trait("EndToEnd/API", "Category/Get - Endpoints")]
    public async Task ErrorWhenCategoryNotFound()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var randomGuid = Guid.NewGuid();

        var (httpMessage, response) = await _fixture.ApiClient
            .Get<ProblemDetails>(
            $"/categories/{randomGuid}"
        );

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Should().NotBeNull();
        response!.Status.Should().Be((int)StatusCodes.Status404NotFound);
        response.Type.Should().Be("NotFound");
        response.Title.Should().Be("Not Found");
        response.Detail.Should().Be($"Category '{randomGuid}' not found.");
    }
}
