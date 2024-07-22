using FC.Codeflix.Catalog.Api.ApiModels.Genre;
using FC.Codeflix.Catalog.Api.ApiModels.Response;
using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Net;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Genre.UpdateGenre;

[Collection(nameof(UpdateGenreApiTestFixture))]
public class UpdateGenreApiTest
{
    private readonly UpdateGenreApiTestFixture _fixture;

    public UpdateGenreApiTest(UpdateGenreApiTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(UpdateGenre))]
    [Trait("EndToEnd/Api", "Genre/UpdateGenre - Endpoints")]
    public async Task UpdateGenre()
    {
        List<DomainEntity.Genre> exampleGenres = _fixture.GetExampleListGenres(10);
        var targetGenre = exampleGenres[5];
        await _fixture.GenrePersistence.InsertList(exampleGenres);
        var input = new UpdateGenreApiRequest(
            _fixture.GetValidGenreName(),
            _fixture.GetRandomBoolean()
        );

        var (response, output) = await _fixture.ApiClient
            .Put<ApiResponse<GenreModelResponse>>(
                $"/genres/{targetGenre.Id}",
                input
            );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Data.Id.Should().Be(targetGenre.Id);
        output.Data.Name.Should().Be(input.Name);
        output.Data.IsActive.Should().Be((bool)input.IsActive!);
        var genreFromDb = await _fixture.GenrePersistence.GetById(output.Data.Id);
        genreFromDb.Should().NotBeNull();
        genreFromDb!.Name.Should().Be(input.Name);
        genreFromDb.IsActive.Should().Be((bool)input.IsActive!);
    }
}
