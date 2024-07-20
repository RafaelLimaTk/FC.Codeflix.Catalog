using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;
using FC.Codeflix.Catalog.Domain.SeedWorks.SearchableRepository;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.ListGenres;
public class ListGenresResponse
    : PaginatedListResponse<GenreModelResponse>
{
    public ListGenresResponse(
        int page,
        int perPage,
        int total,
        IReadOnlyList<GenreModelResponse> items
    )
        : base(page, perPage, total, items)
    { }

    public static ListGenresResponse FromSearchOutput(
        SearchResponse<DomainEntity.Genre> searchOutput
    ) => new(
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Total,
            searchOutput.Items
                .Select(GenreModelResponse.FromGenre)
                .ToList()
        );

    internal void FillWithCategoryNames(IReadOnlyList<DomainEntity.Category> categories)
    {
        foreach (GenreModelResponse item in Items)
            foreach (GenreModelResponseCategory categoryOutput in item.Categories)
                categoryOutput.Name = categories?.FirstOrDefault(
                    category => category.Id == categoryOutput.Id
                )?.Name;
    }
}
