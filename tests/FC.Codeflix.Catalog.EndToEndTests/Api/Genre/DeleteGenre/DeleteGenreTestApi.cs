using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Genre.DeleteGenre;

[Collection(nameof(DeleteGenreTestApiFixture))]
public class DeleteGenreTestApi : IDisposable
{
    private readonly DeleteGenreTestApiFixture _fixture;

    public DeleteGenreTestApi(DeleteGenreTestApiFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(DeleteGenre))]
    [Trait("EndToEnd/Api", "Genre/DeleteGenre - Endpoints")]
    public async Task DeleteGenre()
    {
        List<DomainEntity.Genre> exampleGenres = _fixture.GetExampleListGenres(10);
        var targetGenre = exampleGenres[5];
        await _fixture.GenrePersistence.InsertList(exampleGenres);

        var (response, output) = await _fixture.ApiClient
            .Delete<object>($"/genres/{targetGenre.Id}");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);
        output.Should().BeNull();
        DomainEntity.Genre? genreDb = await _fixture.GenrePersistence.GetById(targetGenre.Id);
        genreDb.Should().BeNull();
    }

    [Fact(DisplayName = nameof(WhenNotFound404))]
    [Trait("EndToEnd/Api", "Genre/DeleteGenre - Endpoints")]
    public async Task WhenNotFound404()
    {
        List<DomainEntity.Genre> exampleGenres = _fixture.GetExampleListGenres(10);
        var randomGuid = Guid.NewGuid();
        await _fixture.GenrePersistence.InsertList(exampleGenres);

        var (response, output) = await _fixture.ApiClient
            .Delete<ProblemDetails>($"/genres/{randomGuid}");

        response.Should().NotBeNull();
        output.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
        output!.Type.Should().Be("NotFound");
        output.Detail.Should().Be($"Genre '{randomGuid}' not found.");
    }

    public void Dispose() => _fixture.CleanPersistence();
}
