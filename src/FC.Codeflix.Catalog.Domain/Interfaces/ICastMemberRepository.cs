using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.SeedWorks;
using FC.Codeflix.Catalog.Domain.SeedWorks.SearchableRepository;

namespace FC.Codeflix.Catalog.Domain.Interfaces;
public interface ICastMemberRepository
        : IGenericRepository<CastMember>,
        ISearchableRepository<CastMember>
{
}
