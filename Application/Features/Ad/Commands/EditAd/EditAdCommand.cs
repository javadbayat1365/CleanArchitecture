using Application.Common;
using Application.Common.Validation;
using FluentValidation;
using Mediator;

namespace Application.Features.Ad.Commands.EditAd;

public record EditAdCommand(
    Guid AdId, 
    string? Title, 
    string? Description, 
    Guid? categotyId, 
    Guid? locationId,
    string[] RemovedImageNames,
    EditAdCommand.AddNewImagesModel[] NewImages) 
    : IRequest<OperationResult<bool>>, IValidatableModel<EditAdCommand>
{
    public record AddNewImagesModel(string ImagesContent,string ImageType);
    public IValidator<EditAdCommand> Validate(ValidationModelBase<EditAdCommand> validator)
    {
        validator.RuleFor(c => c.AdId).NotEmpty();

        return validator;
    }
}
