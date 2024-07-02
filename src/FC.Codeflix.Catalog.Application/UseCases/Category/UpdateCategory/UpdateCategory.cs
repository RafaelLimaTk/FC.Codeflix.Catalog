using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Domain.Interfaces;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
public class UpdateCategory : IUpdateCategory
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _uinitOfWork;

    public UpdateCategory(
        ICategoryRepository categoryRepository,
        IUnitOfWork uinitOfWork)
        => (_categoryRepository, _uinitOfWork)
            = (categoryRepository, uinitOfWork);

    public async Task<CategoryModelResponse> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);
        category.Update(request.Name, request.Description);

        if (request.IsActive.HasValue && request.IsActive.Value != category.IsActive)
        {
            if (request.IsActive.Value) category.Activate();
            else category.Deactivate();
        }

        await _categoryRepository.Update(category, cancellationToken);
        await _uinitOfWork.Commit(cancellationToken);
        return CategoryModelResponse.FromCategory(category);
    }
}
