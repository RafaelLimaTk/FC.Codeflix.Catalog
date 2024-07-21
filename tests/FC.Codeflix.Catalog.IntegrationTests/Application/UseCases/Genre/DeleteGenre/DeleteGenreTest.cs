using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Genre.DeleteGenre;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Genre.DeleteGenre;

[Collection(nameof(DeleteGenreTestFixture))]
public class DeleteGenreTest
{
    private readonly DeleteGenreTestFixture _fixture;

    public DeleteGenreTest(DeleteGenreTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(DeleteGenre))]
    [Trait("Integration/Application", "DeleteGenre - Use Cases")]
    public async Task DeleteGenre()
    {
        var genresExampleList = _fixture.GetExampleListGenres(10);
        var targetGenre = genresExampleList[5];
        var dbArrangeContext = _fixture.CreateDbContext();
        await dbArrangeContext.Genres.AddRangeAsync(genresExampleList);
        await dbArrangeContext.SaveChangesAsync();
        var actDbContext = _fixture.CreateDbContext(true);
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        //var eventPublisher = new DomainEventPublisher(serviceProvider);
        var unitOfWork = new UnitOfWork(actDbContext);
        var useCase = new UseCase.DeleteGenre(
            new GenreRepository(actDbContext),
            unitOfWork
        );
        var input = new UseCase.DeleteGenreRequest(targetGenre.Id);

        await useCase.Handle(input, CancellationToken.None);

        var assertDbContext = _fixture.CreateDbContext(true);
        var genreFromDb = await assertDbContext.Genres.FindAsync(targetGenre.Id);
        genreFromDb.Should().BeNull();
    }
}
