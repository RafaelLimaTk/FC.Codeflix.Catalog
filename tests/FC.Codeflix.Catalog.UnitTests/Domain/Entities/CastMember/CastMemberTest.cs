namespace FC.Codeflix.Catalog.UnitTests.Domain.Entities.CastMember;

using FluentAssertions;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;

[Collection(nameof(CastMemberTestFixture))]
public class CastMemberTest
{
    private CastMemberTestFixture _fixture;

    public CastMemberTest(CastMemberTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "CastMember - Aggregates")]
    public void Instantiate()
    {
        var datetimeBefore = DateTime.Now.AddSeconds(-1);
        var name = _fixture.GetValidName();
        var type = _fixture.GetRandomCastMemberType();

        var castMember = new DomainEntity.CastMember(name, type);

        var datetimeAfter = DateTime.Now.AddSeconds(1);
        castMember.Id.Should().NotBe(default(Guid));
        castMember.Name.Should().Be(name);
        castMember.Type.Should().Be(type);
        (castMember.CreatedAt >= datetimeBefore).Should().BeTrue();
        (castMember.CreatedAt <= datetimeAfter).Should().BeTrue();
    }
}
