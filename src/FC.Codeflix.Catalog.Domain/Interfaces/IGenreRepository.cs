using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.SeedWorks;
using FC.Codeflix.Catalog.Domain.SeedWorks.SearchableRepository;

namespace FC.Codeflix.Catalog.Domain.Interfaces;
public interface IGenreRepository
    : IGenericRepository<Genre>,
    ISearchableRepository<Genre>
{
    public Task<IReadOnlyList<Guid>> GetIdsListByIds(
        List<Guid> ids,
        CancellationToken cancellationToken
    );

    public Task<IReadOnlyList<Genre>> GetListByIds(
        List<Guid> ids,
        CancellationToken cancellationToken
    );
}
