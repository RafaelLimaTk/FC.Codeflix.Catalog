using FC.Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;
using FC.Codeflix.Catalog.UnitTests.Application.Genre.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.Genre.CreateGenre;

[CollectionDefinition(nameof(CreateGenreTestFixture))]
public class CreateGenreTestFixtureCollection
    : ICollectionFixture<CreateGenreTestFixture>
{ }

public class CreateGenreTestFixture
    : GenreUseCasesBaseFixture
{
    public CreateGenreRequest GetExampleInput()
    => new CreateGenreRequest(
        GetValidGenreName(),
        GetRandomBoolean()
    );
    public CreateGenreRequest GetExampleInput(string? name)
        => new CreateGenreRequest(
            name!,
            GetRandomBoolean()
        );

    public CreateGenreRequest GetExampleInputWithCategories()
    {
        var numberOfCategoriesIds = (new Random()).Next(1, 10);
        var categoriesIds = Enumerable.Range(1, numberOfCategoriesIds)
            .Select(_ => Guid.NewGuid())
            .ToList();
        return new(
            GetValidGenreName(),
            GetRandomBoolean(),
            categoriesIds
        );
    }
}
