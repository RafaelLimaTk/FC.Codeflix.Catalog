using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
public class GetCategoryRequest : IRequest<CategoryModelResponse>
{
    public Guid Id { get; set; }
    public GetCategoryRequest(Guid id)
        => Id = id;
}
