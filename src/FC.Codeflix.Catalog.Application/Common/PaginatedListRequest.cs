using FC.Codeflix.Catalog.Domain.SeedWorks.SearchableRepository;

namespace FC.Codeflix.Catalog.Application.Common;
public abstract class PaginatedListRequest
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public string Search { get; set; }
    public string Sort { get; set; }
    public SearchOrder Dir { get; set; }
    public PaginatedListRequest(
        int page,
        int perPage,
        string search,
        string sort,
        SearchOrder dir)
    {
        Page = page;
        PerPage = perPage;
        Search = search;
        Sort = sort;
        Dir = dir;
    }

    public SearchRequest ToSearchRequest()
        => new(Page, PerPage, Search, Sort, Dir);
}
