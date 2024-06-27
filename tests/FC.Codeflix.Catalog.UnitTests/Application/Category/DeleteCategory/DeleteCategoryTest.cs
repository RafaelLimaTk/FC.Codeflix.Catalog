using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTest
{
    private readonly DeleteCategoryTestFixture _fixture;

    public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Application", "DeleteCategory - Use Cases")]
    public async Task DeleteCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var categoryExample = _fixture.GetExampleCategory();
        repositoryMock.Setup(x => x.Get(
            categoryExample.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(categoryExample);

        var request = new UseCase.DeleteCategoryRequest(categoryExample.Id);

        var useCase = new UseCase.DeleteCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        await useCase.Handle(request, CancellationToken.None);

        repositoryMock.Verify(x => x.Get(
            categoryExample.Id,
            It.IsAny<CancellationToken>()
        ), Times.Once);
        repositoryMock.Verify(x => x.Delete(
            categoryExample,
            It.IsAny<CancellationToken>()
        ), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
