using Application.Common;
using Application.Common.Validation;
using FluentValidation;
using Mediator;

namespace Application.Features.Category.Commands.CreateCategory;

public record CreateCategoryCommand(string categoryName) : IRequest<OperationResult<bool>>, IValidatableModel<CreateCategoryCommand>
{
    public IValidator<CreateCategoryCommand> Validate(ValidationModelBase<CreateCategoryCommand> validator)
    {
        validator.RuleFor(c => c.categoryName).NotEmpty();
        return validator;
    }
}
