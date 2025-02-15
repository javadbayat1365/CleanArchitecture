using Application.Common;
using Application.Contracts.FileService.Interfaces;
using Application.Repositories.Common;
using AutoMapper;
using Domain.Entities.Ad;
using Mediator;
using static Application.Features.Ad.Queries.GetAdById.GetAdDetailByIdQueryResult;

namespace Application.Features.Ad.Queries.GetAdById;

public class GetAdByIdQueryHandler(IUnitOfWork unitOfWork,IFileService fileService,IMapper mapper) : IRequestHandler<GetAdDetailByIdQuery, OperationResult<GetAdDetailByIdQueryResult>>
{
    public async ValueTask<OperationResult<GetAdDetailByIdQueryResult>> Handle(GetAdDetailByIdQuery request, CancellationToken cancellationToken)
    {
        var adEntity =await unitOfWork.AdRepository
            .GetAdDetailByIdAsync(request.AdId,cancellationToken);
        if (adEntity is null)
            return OperationResult<GetAdDetailByIdQueryResult>.NotFoundResult(nameof(GetAdDetailByIdQuery.AdId), "Ad Not Found!");

        var adImages = await fileService.GetFilesByNameAsync(adEntity.Images.Select(s => s.FileName).ToList(),cancellationToken);

        var result = mapper.Map<AdEntitiy,GetAdDetailByIdQueryResult>(adEntity);

        result.AdImages = adImages.Select(s =>new AdDetailsImageModel(s.FileName,s.FileUrl)).ToArray();

        return OperationResult<GetAdDetailByIdQueryResult>.SuccessResult(result);
    }
}
