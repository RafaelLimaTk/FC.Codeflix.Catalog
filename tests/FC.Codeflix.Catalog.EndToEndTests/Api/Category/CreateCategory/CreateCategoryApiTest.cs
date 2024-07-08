using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FluentAssertions;
using System.Net;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.CreateCategory;

[Collection(nameof(CreateCategoryApiTestFixture))]
public class CreateCategoryApiTest
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
            .Post<CategoryModelResponse>(
            "/categories",
            input
        );

        httpMessage.Should().NotBeNull();
        httpMessage!.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Should().NotBeNull();
        response!.Id.Should().NotBeEmpty();
        response.Name.Should().Be(input.Name);
        response.Description.Should().Be(input.Description);
        response.IsActive.Should().Be(input.IsActive);
        response.CreatedAt.Should()
            .NotBeSameDateAs(default(DateTime));
        var dbCategory = await _fixture.Persistence
            .GetById(response.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().NotBeEmpty();
        dbCategory.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(input.IsActive);
        dbCategory.CreatedAt.Should()
            .NotBeSameDateAs(default(DateTime));
    }
}
