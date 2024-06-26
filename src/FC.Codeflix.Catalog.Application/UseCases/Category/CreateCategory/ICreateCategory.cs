using FC.Codeflix.Catalog.Application.UseCases.Category.Common;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
public interface ICreateCategory
{
    Task<CategoryModelResponse> Handle(CreateCategoryRequest request, CancellationToken cancellationToken);
}
