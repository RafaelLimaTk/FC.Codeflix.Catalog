using FC.Codeflix.Catalog.Domain.Interfaces;
using FC.Codeflix.Catalog.Domain.SeedWorks.SearchableRepository;
using FluentAssertions;
using Moq;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.CastMember.ListCastMembers;

namespace FC.Codeflix.Catalog.UnitTests.Application.CastMember.ListCastMembers;

[Collection(nameof(ListCastMembersTestFixture))]
public class ListCastMembersTest
{
    private readonly ListCastMembersTestFixture _fixture;

    public ListCastMembersTest(ListCastMembersTestFixture fixture) => _fixture = fixture;

    [Fact(DisplayName = nameof(List))]
    [Trait("Application", "ListCastMembers - Use Cases")]
    public async Task List()
    {
        var repositoryMock = new Mock<ICastMemberRepository>();
        var castMembersListExample = _fixture.GetExampleCastMembersList(3);
        var repositorySearchResponse = new SearchResponse<DomainEntity.CastMember>(
            1, 10, castMembersListExample.Count,
            (IReadOnlyList<DomainEntity.CastMember>)castMembersListExample
        );
        repositoryMock.Setup(x => x.Search(
            It.IsAny<SearchRequest>(), It.IsAny<CancellationToken>()
        )).ReturnsAsync(repositorySearchResponse);
        var input = new UseCase.ListCastMembersRequest(1, 10, "", "", SearchOrder.Asc);
        var useCase = new UseCase.ListCastMembers(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(repositorySearchResponse.CurrentPage);
        output.PerPage.Should().Be(repositorySearchResponse.PerPage);
        output.Total.Should().Be(repositorySearchResponse.Total);
        output.Items.ToList().ForEach(outputItem =>
        {
            var example = castMembersListExample.Find(x => x.Id == outputItem.Id);
            example.Should().NotBeNull();
            outputItem.Name.Should().Be(example.Name);
            outputItem.Type.Should().Be(example.Type);
        });
        repositoryMock.Verify(x => x.Search(
            It.Is<SearchRequest>(x => (
                x.Page == input.Page
                && x.PerPage == input.PerPage
                && x.Search == input.Search
                && x.Order == input.Dir
                && x.OrderBy == input.Sort
            )),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Fact(DisplayName = nameof(RetursEmptyWhenIsEmpty))]
    [Trait("Application", "ListCastMembers - Use Cases")]
    public async Task RetursEmptyWhenIsEmpty()
    {
        var repositoryMock = new Mock<ICastMemberRepository>();
        var castMembersListExample = new List<DomainEntity.CastMember>();
        var repositorySearchResponse = new SearchResponse<DomainEntity.CastMember>(
            1, 10, castMembersListExample.Count,
            (IReadOnlyList<DomainEntity.CastMember>)castMembersListExample
        );
        repositoryMock.Setup(x => x.Search(
            It.IsAny<SearchRequest>(), It.IsAny<CancellationToken>()
        )).ReturnsAsync(repositorySearchResponse);
        var input = new UseCase.ListCastMembersRequest(1, 10, "", "", SearchOrder.Asc);
        var useCase = new UseCase.ListCastMembers(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(repositorySearchResponse.CurrentPage);
        output.PerPage.Should().Be(repositorySearchResponse.PerPage);
        output.Total.Should().Be(repositorySearchResponse.Total);
        output.Items.Should().HaveCount(castMembersListExample.Count);
        repositoryMock.Verify(x => x.Search(
            It.Is<SearchRequest>(x => (
                x.Page == input.Page
                && x.PerPage == input.PerPage
                && x.Search == input.Search
                && x.Order == input.Dir
                && x.OrderBy == input.Sort
            )),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
