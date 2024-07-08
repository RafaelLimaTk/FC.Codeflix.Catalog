using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryApiTestFixture))]
public class UpdateCategoryApiTest
{
    private readonly UpdateCategoryApiTestFixture _fixture;

    public UpdateCategoryApiTest(UpdateCategoryApiTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(UpdateCategory))]
    [Trait("EndToEnd/API", "Category/Update - Endpoints")]
    public async void UpdateCategory()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var exampleCategory = exampleCategoriesList[10];
        var input = _fixture.GetExampleInput();

        var (httpMessage, reponse) = await _fixture.ApiClient.Put<CategoryModelResponse>(
            $"/categories/{exampleCategory.Id}",
            input
        );

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        reponse.Should().NotBeNull();
        reponse!.Should().NotBeNull();
        reponse!.Id.Should().Be(exampleCategory.Id);
        reponse.Name.Should().Be(input.Name);
        reponse.Description.Should().Be(input.Description);
        reponse.IsActive.Should().Be((bool)input.IsActive!);
        var dbCategory = await _fixture
            .Persistence.GetById(exampleCategory.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be((bool)input.IsActive!);
    }

    [Fact(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("EndToEnd/API", "Category/Update - Endpoints")]
    public async void UpdateCategoryOnlyName()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var exampleCategory = exampleCategoriesList[10];
        var input = new UpdateCategoryRequest(
            exampleCategory.Id,
            _fixture.GetValidCategoryName()
        );

        var (httpMessage, reponse) = await _fixture.ApiClient.Put<CategoryModelResponse>(
            $"/categories/{exampleCategory.Id}",
            input
        );

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        reponse.Should().NotBeNull();
        reponse!.Should().NotBeNull();
        reponse!.Id.Should().Be(exampleCategory.Id);
        reponse.Name.Should().Be(input.Name);
        reponse.Description.Should().Be(exampleCategory.Description);
        reponse.IsActive.Should().Be(exampleCategory.IsActive);
        var dbCategory = await _fixture
            .Persistence.GetById(exampleCategory.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
    }

    [Fact(DisplayName = nameof(UpdateCategoryNameAndDescription))]
    [Trait("EndToEnd/API", "Category/Update - Endpoints")]
    public async void UpdateCategoryNameAndDescription()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var exampleCategory = exampleCategoriesList[10];
        var input = new UpdateCategoryRequest(
            exampleCategory.Id,
            _fixture.GetValidCategoryName(),
            _fixture.GetValidCategoryDescription()
        );

        var (httpMessage, reponse) = await _fixture.ApiClient.Put<CategoryModelResponse>(
            $"/categories/{exampleCategory.Id}",
            input
        );

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        reponse.Should().NotBeNull();
        reponse!.Should().NotBeNull();
        reponse!.Id.Should().Be(exampleCategory.Id);
        reponse.Name.Should().Be(input.Name);
        reponse.Description.Should().Be(input.Description);
        reponse.IsActive.Should().Be(exampleCategory.IsActive);
        var dbCategory = await _fixture
            .Persistence.GetById(exampleCategory.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
    }

    [Fact(DisplayName = nameof(ErrorWhenNotFound))]
    [Trait("EndToEnd/API", "Category/Update - Endpoints")]
    public async void ErrorWhenNotFound()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var randomGuid = Guid.NewGuid();
        var input = _fixture.GetExampleInput();

        var (httpMessage, reponse) = await _fixture.ApiClient.Put<ProblemDetails>(
            $"/categories/{randomGuid}",
            input
        );

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
        reponse.Should().NotBeNull();
        reponse!.Title.Should().Be("Not Found");
        reponse.Type.Should().Be("NotFound");
        reponse.Status.Should().Be((int)StatusCodes.Status404NotFound);
        reponse.Detail.Should().Be($"Category '{randomGuid}' not found.");
    }
}
