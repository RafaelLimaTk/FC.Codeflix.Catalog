using FC.Codeflix.Catalog.Application.Exceptions;
using FluentAssertions;
using Moq;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.GetCategory;

[Collection(nameof(GetCategoryTestFixture))]
public class GetCategoryTest
{
    private readonly GetCategoryTestFixture _fixture;

    public GetCategoryTest(GetCategoryTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(GetCategory))]
    [Trait("Application", "GetCategory - Use Cases")]
    public async Task GetCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleCategory = _fixture.GetExampleCategory();

        repositoryMock.Setup(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleCategory);

        var request = new UseCase.GetCategoryRequest(exampleCategory.Id);
        var useCase = new UseCase.GetCategory(repositoryMock.Object);

        var response = await useCase.Handle(request, CancellationToken.None);

        repositoryMock.Verify(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);

        response.Should().NotBeNull();
        response.Id.Should().Be(exampleCategory.Id);
        response.Name.Should().Be(exampleCategory.Name);
        response.Description.Should().Be(exampleCategory.Description);
        response.IsActive.Should().Be(exampleCategory.IsActive);
        response.CreatedAt.Should().Be(exampleCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesntExist))]
    [Trait("Application", "GetCategory - Use Cases")]
    public void NotFoundExceptionWhenCategoryDoesntExist()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleGuid = Guid.NewGuid();

        repositoryMock.Setup(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ThrowsAsync(
            new NotFoundException($"Category '{exampleGuid}' not found")
        );

        var request = new UseCase.GetCategoryRequest(exampleGuid);
        var useCase = new UseCase.GetCategory(repositoryMock.Object);

        Func<Task> act = async ()
            => await useCase.Handle(request, CancellationToken.None);

        act.Should().ThrowAsync<NotFoundException>();
        repositoryMock.Verify(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
