using FC.Codeflix.Catalog.Application.UseCases.Genre.ListGenres;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FluentAssertions;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Genre.ListGenres;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Genre.ListGenres;

[Collection(nameof(ListGenresTestFixture))]
public class ListGenresTest
{
    private readonly ListGenresTestFixture _fixture;

    public ListGenresTest(ListGenresTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(ListGenres))]
    [Trait("Integration/Application", "ListGenres - UseCases")]
    public async Task ListGenres()
    {
        List<DomainEntity.Genre> exampleGenres = _fixture.GetExampleListGenres(10);
        CodeflixCatalogDbContext arrangeDbContext = _fixture.CreateDbContext();
        await arrangeDbContext.AddRangeAsync(exampleGenres);
        await arrangeDbContext.SaveChangesAsync();
        CodeflixCatalogDbContext actDbContext = _fixture.CreateDbContext(true);
        UseCase.ListGenres useCase = new UseCase.ListGenres(
            new GenreRepository(actDbContext),
            new CategoryRepository(actDbContext)
        );
        UseCase.ListGenresRequest input = new UseCase.ListGenresRequest(1, 20);

        ListGenresResponse output = await useCase.Handle(
            input,
            CancellationToken.None
        );

        output.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(exampleGenres.Count);
        output.Items.Should().HaveCount(exampleGenres.Count);
        output.Items.ToList().ForEach(outputItem =>
        {
            DomainEntity.Genre? exampleItem =
                exampleGenres.Find(example => example.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
        });
    }
}
