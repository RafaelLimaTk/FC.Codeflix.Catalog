﻿using FC.Codeflix.Catalog.UnitTests.Common;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.UnitTests.Application.Genre.Common;

public class GenreUseCasesBaseFixture
    : BaseFixture
{
    public string GetValidGenreName()
        => Faker.Commerce.Categories(1)[0];

    public DomainEntity.Genre GetExampleGenre(
        bool? isActive = null,
        List<Guid>? categoriesIds = null
    )
    {
        var genre = new DomainEntity.Genre(
            GetValidGenreName(),
            isActive ?? GetRandomBoolean()
        );
        categoriesIds?.ForEach(genre.AddCategory);
        return genre;
    }

    public List<DomainEntity.Genre> GetExampleGenresList(
        int count = 10
    )
        => Enumerable.Range(1, count).Select(_ =>
    {
        var genre = new DomainEntity.Genre(
            GetValidGenreName(),
            GetRandomBoolean()
        );
        GetRandomIdsList()
            .ForEach(genre.AddCategory);
        return genre;
    }).ToList();

    public List<Guid> GetRandomIdsList(int? count = null)
        => Enumerable
            .Range(1, count ?? (new Random()).Next(1, 10))
            .Select(_ => Guid.NewGuid())
            .ToList();
}
