using Application.Common;
using Application.Common.Validation;
using FluentValidation;
using Mediator;

namespace Application.Features.Ad.Commands;

public record CreateAdCommand(
    Guid UserId,
    Guid CategoryId,
    Guid LocationId,
    string Title,
    string Description,
    CreateAdCommand.CreateAdImagesModel[] AdImages):IRequest<OperationResult<bool>>,IValidatableModel<CreateAdCommand>
{
    public IValidator<CreateAdCommand> Validate(ValidationModelBase<CreateAdCommand> validator)
    {
        validator.RuleFor(x => x.UserId).NotEmpty();
        validator.RuleFor(x => x.CategoryId).NotEmpty();
        validator.RuleFor(x => x.LocationId).NotEmpty();
        validator.RuleFor(x => x.Description).NotEmpty();
        validator.RuleFor(x => x.Title).NotEmpty();

        return validator;
    }

    public record CreateAdImagesModel(string Base64File,string FileContent);

}

