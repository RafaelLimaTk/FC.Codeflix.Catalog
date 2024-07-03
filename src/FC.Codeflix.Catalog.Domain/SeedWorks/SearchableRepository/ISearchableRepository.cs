namespace FC.Codeflix.Catalog.Domain.SeedWorks.SearchableRepository;
public interface ISearchableRepository<Taggregate> where Taggregate : AggregateRoot
{
    Task<SearchResponse<Taggregate>> Search(
    SearchRequest input,
    CancellationToken cancellationToken
);
}
