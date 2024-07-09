using FC.Codeflix.Catalog.Api.ApiModels.Category;
using FC.Codeflix.Catalog.Api.ApiModels.Response;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryApiTestFixture))]
public class UpdateCategoryApiTest
    : IDisposable
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

        var (httpMessage, response) = await _fixture.ApiClient.Put<ApiResponse<CategoryModelResponse>>(
            $"/categories/{exampleCategory.Id}",
            input
        );

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Data.Should().NotBeNull();
        response.Data.Id.Should().Be(exampleCategory.Id);
        response.Data.Name.Should().Be(input.Name);
        response.Data.Description.Should().Be(input.Description);
        response.Data.IsActive.Should().Be((bool)input.IsActive!);
        var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
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
        var input = new UpdateCategoryApiRequest(
            _fixture.GetValidCategoryName()
        );

        var (httpMessage, response) = await _fixture.ApiClient.Put<ApiResponse<CategoryModelResponse>>(
            $"/categories/{exampleCategory.Id}",
            input
        );

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Data.Should().NotBeNull();
        response.Data.Id.Should().Be(exampleCategory.Id);
        response.Data.Name.Should().Be(input.Name);
        response.Data.Description.Should().Be(exampleCategory.Description);
        response.Data.IsActive.Should().Be(exampleCategory.IsActive);
        var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
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
        var input = new UpdateCategoryApiRequest(
            _fixture.GetValidCategoryName(),
            _fixture.GetValidCategoryDescription()
        );

        var (httpMessage, response) = await _fixture.ApiClient.Put<ApiResponse<CategoryModelResponse>>(
            $"/categories/{exampleCategory.Id}",
            input
        );

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Data.Should().NotBeNull();
        response.Data.Id.Should().Be(exampleCategory.Id);
        response.Data.Name.Should().Be(input.Name);
        response.Data.Description.Should().Be(input.Description);
        response.Data.IsActive.Should().Be(exampleCategory.IsActive);
        var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
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

        var (httpMessage, response) = await _fixture.ApiClient.Put<ProblemDetails>(
            $"/categories/{randomGuid}",
            input
        );

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
        response.Should().NotBeNull();
        response!.Title.Should().Be("Not Found");
        response.Type.Should().Be("NotFound");
        response.Status.Should().Be((int)StatusCodes.Status404NotFound);
        response.Detail.Should().Be($"Category '{randomGuid}' not found.");
    }

    [Theory(DisplayName = nameof(ErrorWhenCantInstantiateAggregate))]
    [Trait("EndToEnd/API", "Category/Update - Endpoints")]
    [MemberData(
    nameof(UpdateCategoryApiTestDataGenerator.GetInvalidInputs),
    MemberType = typeof(UpdateCategoryApiTestDataGenerator)
    )]
    public async void ErrorWhenCantInstantiateAggregate(
        UpdateCategoryApiRequest input,
        string expectedDetail
    )
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var exampleCategory = exampleCategoriesList[10];

        var (httpMessage, response) = await _fixture.ApiClient.Put<ProblemDetails>(
            $"/categories/{exampleCategory.Id}",
            input
        );

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status422UnprocessableEntity);
        response.Should().NotBeNull();
        response!.Title.Should().Be("One or more validation errors occurred.");
        response.Type.Should().Be("UnprocessableEntity");
        response.Status.Should().Be((int)StatusCodes.Status422UnprocessableEntity);
        response.Detail.Should().Be(expectedDetail);
    }

    public void Dispose()
        => _fixture.CleanPersistence();
}
