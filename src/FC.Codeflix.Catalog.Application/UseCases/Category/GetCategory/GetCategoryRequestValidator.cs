using FluentValidation;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
public class GetCategoryRequestValidator
    : AbstractValidator<GetCategoryRequest>
{
    public GetCategoryRequestValidator()
        => RuleFor(x => x.Id).NotEmpty();
}
