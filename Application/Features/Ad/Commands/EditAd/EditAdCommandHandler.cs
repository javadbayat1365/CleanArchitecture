using Application.Common;
using Application.Contracts.FileService.Interfaces;
using Application.Contracts.FileService.Models;
using Application.Repositories.Common;
using Domain.Entities.Ad;
using Mediator;

namespace Application.Features.Ad.Commands.EditAd;

public sealed class EditAdCommandHandler(IUnitOfWork unitOfWork,IFileService fileService) : IRequestHandler<EditAdCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(EditAdCommand request, CancellationToken cancellationToken)
    {
        if (request.categotyId.HasValue && request.categotyId.Value != Guid.Empty) 
        {
           var category =await unitOfWork.CategoryRepository.GetCategoryByIdAsync(request.categotyId.Value,cancellationToken);
            if (category is null)
            {
                return OperationResult<bool>.NotFoundResult(nameof(request.categotyId),"Category Not Found!");
            }
        }
        if (request.locationId.HasValue && request.locationId.Value != Guid.Empty)
        {
           var location =await unitOfWork.LocationRepository.GetLocationByIdAsync(request.locationId.Value,cancellationToken);
            if (location is null)
            {
                return OperationResult<bool>.NotFoundResult(nameof(request.locationId), "Location Not Found!");
            }
        }

        AdEntitiy editAd =await unitOfWork.AdRepository.GetAdByIdForUpdateAsync(request.AdId,cancellationToken);
        if(editAd is null)
        {
            return OperationResult<bool>.NotFoundResult(nameof(EditAdCommand.AdId), "Ad Not Found!");
        }
        editAd.Edit(request.Title,request.Description,request.categotyId.Value,request.locationId.Value);

        if(request.RemovedImageNames.Any())
        {
            await fileService.RemoveFileAsync(request.RemovedImageNames,cancellationToken);
            editAd.RemoveImages(request.RemovedImageNames);
        }

        if (request.NewImages.Any())
        {
            foreach (var item in request.NewImages)
            {
                var savedImages = await fileService.SaveFilesAsync(new List<SaveFileModel>(
                request.NewImages.Select(s => new SaveFileModel(s.ImagesContent , s.ImageType)).ToList()), cancellationToken);

                savedImages.ForEach(a => editAd.AddImage(new Domain.Common.ValueObjects.ImageValueObject(a.FileName, a.FileType)));
            }
        }

        await unitOfWork.CommitAsync(cancellationToken);

        return OperationResult<bool>.SuccessResult(true);
    }
}
 