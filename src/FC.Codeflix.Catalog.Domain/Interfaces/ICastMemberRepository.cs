using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.SeedWorks;
using FC.Codeflix.Catalog.Domain.SeedWorks.SearchableRepository;

namespace FC.Codeflix.Catalog.Domain.Interfaces;

public interface ICastMemberRepository
    : IGenericRepository<CastMember>,
    ISearchableRepository<CastMember>
{
    public Task<IReadOnlyList<Guid>> GetIdsListByIds(
        List<Guid> ids,
        CancellationToken cancellationToken
    );
}