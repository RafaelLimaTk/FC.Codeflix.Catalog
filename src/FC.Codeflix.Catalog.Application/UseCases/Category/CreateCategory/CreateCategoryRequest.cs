using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
public class CreateCategoryRequest : IRequest<CategoryModelResponse>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }

    public CreateCategoryRequest(
        string name,
        string? description = null,
        bool isActive = true
        )
    {
        Name = name;
        Description = description ?? "";
        IsActive = isActive;
    }
}
