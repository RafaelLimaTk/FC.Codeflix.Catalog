using FluentValidation;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
public class UpdateCategoryRequestValidator
    : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
    => RuleFor(x => x.Id).NotEmpty();
}
