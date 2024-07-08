using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryApiTestFixture))]
public class DeleteCategoryApiTest
    : IDisposable
{
    private readonly DeleteCategoryApiTestFixture _fixture;

    public DeleteCategoryApiTest(DeleteCategoryApiTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("EndToEnd/API", "Category/Delete - Endpoints")]
    public async void DeleteCategory()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var exampleCategory = exampleCategoriesList[10];

        var (httpMessage, response) = await _fixture.ApiClient.Delete<object>(
            $"/categories/{exampleCategory.Id}"
        );

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should()
            .Be((HttpStatusCode)StatusCodes.Status204NoContent);
        response.Should().BeNull();
        var persistenceCategory = await _fixture.Persistence
            .GetById(exampleCategory.Id);
        persistenceCategory.Should().BeNull();
    }

    [Fact(DisplayName = nameof(ErrorWhenNotFound))]
    [Trait("EndToEnd/API", "Category/Delete - Endpoints")]
    public async void ErrorWhenNotFound()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var randomGuid = Guid.NewGuid();

        var (httpMessage, response) = await _fixture.ApiClient.Delete<ProblemDetails>(
            $"/categories/{randomGuid}"
        );

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should()
            .Be((HttpStatusCode)StatusCodes.Status404NotFound);
        response.Should().NotBeNull();
        response!.Title.Should().Be("Not Found");
        response!.Type.Should().Be("NotFound");
        response!.Status.Should().Be((int)StatusCodes.Status404NotFound);
        response!.Detail.Should().Be($"Category '{randomGuid}' not found.");
    }

    public void Dispose()
        => _fixture.CleanPersistence();
}
