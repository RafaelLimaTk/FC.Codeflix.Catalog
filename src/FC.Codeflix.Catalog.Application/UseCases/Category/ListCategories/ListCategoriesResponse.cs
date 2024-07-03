using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
public class ListCategoriesResponse
    : PaginatedListResponse<CategoryModelResponse>
{
    public ListCategoriesResponse(
        int page,
        int perPage,
        int total,
        IReadOnlyList<CategoryModelResponse> items)
        : base(page, perPage, total, items)
    {
    }
}
