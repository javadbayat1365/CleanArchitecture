using Application.Common;
using Application.Contracts.FileService.Interfaces;
using Application.Contracts.FileService.Models;
using Application.Contracts.User;
using Application.Repositories.Common;
using Domain.Entities.Ad;
using Mediator;

namespace Application.Features.Ad.Commands;

public class CreateAdCommandHandler(IUnitOfWork unitOfWork, IUserManager userManager, IFileService fileService) : IRequestHandler<CreateAdCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(CreateAdCommand request, CancellationToken cancellationToken)
    {
        var location = await unitOfWork.LocationRepository.GetLocationByIdAsync(request.LocationId, cancellationToken);
        if (location == null)
        {
            return OperationResult<bool>.NotFoundResult(nameof(request.LocationId), "Location Is Not Found");
        }
        var category = await unitOfWork.CategoryRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
        {
            return OperationResult<bool>.NotFoundResult(nameof(request.CategoryId), "Category Is Not Found");
        }
        var user = await userManager.GetUserByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return OperationResult<bool>.NotFoundResult(nameof(request.UserId), "User Is Not Found");
        }
        AdEntitiy createAd;
        try
        {
            createAd = AdEntitiy.Create(request.Title, request.Description, request.UserId, request.CategoryId, request.LocationId);
        }
        catch (Exception e)
        {
            return OperationResult<bool>.FailureResult(e.Message);
        }
        if (request.AdImages.Any())
        {
            var savedImages = await fileService.SaveFilesAsync(new List<SaveFileModel>(
                 request.AdImages.Select(s => new SaveFileModel(s.Base64File, s.FileContent)).ToList()), cancellationToken);

            savedImages.ForEach(a => createAd.AddImage(new Domain.Common.ValueObjects.ImageValueObject(a.FileName, a.FileType)));
        }

        await unitOfWork.CommitAsync(cancellationToken);

        return OperationResult<bool>.SuccessResult(true);
    }
}
