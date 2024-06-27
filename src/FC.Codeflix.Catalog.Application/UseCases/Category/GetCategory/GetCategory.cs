using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Domain.Interfaces;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
public class GetCategory : IGetCategory
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategory(ICategoryRepository categoryRepository)
        => _categoryRepository = categoryRepository;

    public async Task<CategoryModelResponse> Handle(
        GetCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);
        return CategoryModelResponse.FromCategory(category);
    }
}
