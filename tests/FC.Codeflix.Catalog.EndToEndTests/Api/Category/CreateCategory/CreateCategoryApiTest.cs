using FC.Codeflix.Catalog.Api.ApiModels.Response;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.CreateCategory;

[Collection(nameof(CreateCategoryApiTestFixture))]
public class CreateCategoryApiTest
    : IDisposable
{
    private readonly CreateCategoryApiTestFixture _fixture;

    public CreateCategoryApiTest(CreateCategoryApiTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("EndToEnd/API", "Category/Create - Endpoints")]
    public async Task CreateCategory()
    {
        var input = _fixture.getExampleInput();

        var (httpMessage, response) = await _fixture.ApiClient
            .Post<ApiResponse<CategoryModelResponse>>(
            "/categories",
            input
        );

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Should().NotBeNull();
        response!.Data.Should().NotBeNull();
        response.Data.Id.Should().NotBeEmpty();
        response.Data.Name.Should().Be(input.Name);
        response.Data.Description.Should().Be(input.Description);
        response.Data.IsActive.Should().Be(input.IsActive);
        response.Data.CreatedAt.Should()
            .NotBeSameDateAs(default(DateTime));
        var dbCategory = await _fixture.Persistence
            .GetById(response.Data.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().NotBeEmpty();
        dbCategory.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(input.IsActive);
        dbCategory.CreatedAt.Should()
            .NotBeSameDateAs(default(DateTime));
    }

    [Theory(DisplayName = nameof(ErrorWhenCantInstantiateAggregate))]
    [Trait("EndToEnd/API", "Category/Create - Endpoints")]
    [MemberData(
        nameof(CreateCategoryApiTestDataGenerator.GetInvalidInputs),
        MemberType = typeof(CreateCategoryApiTestDataGenerator)
    )]
    public async Task ErrorWhenCantInstantiateAggregate(
        CreateCategoryRequest input,
        string expectedDetail
    )
    {
        var (httpMessage, response) = await _fixture.ApiClient
            .Post<ProblemDetails>(
            "/categories",
            input
        );

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        response.Should().NotBeNull();
        response!.Title.Should().Be("One or more validation errors occurred.");
        response.Type.Should().Be("UnprocessableEntity");
        response.Status.Should().Be((int)StatusCodes.Status422UnprocessableEntity);
        response.Detail.Should().Be(expectedDetail);
    }

    public void Dispose()
        => _fixture.CleanPersistence();
}
