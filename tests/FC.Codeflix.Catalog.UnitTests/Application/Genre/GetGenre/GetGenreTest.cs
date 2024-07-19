using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;
using FluentAssertions;
using Moq;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Genre.GetGenre;

namespace FC.Codeflix.Catalog.UnitTests.Application.Genre.GetGenre;

[Collection(nameof(GetGenreTestFixture))]
public class GetGenreTest
{
    private readonly GetGenreTestFixture _fixture;

    public GetGenreTest(GetGenreTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(GetGenre))]
    [Trait("Application", "GetGenre - Use Cases")]
    public async Task GetGenre()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        var exempleCategories = _fixture.GetExampleCategoriesList();
        var exempleGenre = _fixture.GetExampleGenre(
            categoriesIds: exempleCategories.Select(x => x.Id).ToList()
        );

        genreRepositoryMock.Setup(x => x.Get(
            It.Is<Guid>(x => x == exempleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exempleGenre);
        categoryRepositoryMock.Setup(x => x.GetListByIds(
            It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()
        )).ReturnsAsync(exempleCategories);
        var useCase = new UseCase
            .GetGenre(genreRepositoryMock.Object, categoryRepositoryMock.Object);
        var input = new UseCase.GetGenreRequest(exempleGenre.Id);

        GenreModelResponse output =
            await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(exempleGenre.Id);
        output.Name.Should().Be(exempleGenre.Name);
        output.IsActive.Should().Be(exempleGenre.IsActive);
        output.CreatedAt.Should().BeSameDateAs(exempleGenre.CreatedAt);
        output.Categories.Should().HaveCount(exempleCategories.Count);
        foreach (var category in output.Categories)
        {
            var expectedCategory = exempleCategories
                .Single(x => x.Id == category.Id);
            expectedCategory.Should().NotBeNull();
            category.Name.Should().Be(expectedCategory.Name);
        }
        genreRepositoryMock.Verify(
                x => x.Get(
                    It.Is<Guid>(x => x == exempleGenre.Id),
                    It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }
}
