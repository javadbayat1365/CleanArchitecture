using Application.Common;
using Application.Repositories.Common;
using Domain.Entities.Ad;
using Mediator;

namespace Application.Features.Location.Commands;

public class CreateLocationCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateLocationCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.LocationRepository.IsLocationExistAsync(request.LocationName, cancellationToken))
            return OperationResult<bool>.FailureResult(nameof(request.LocationName), "Location name already exist!");

        var locaiton = new LocationEntity(request.LocationName);
        await unitOfWork.LocationRepository.CreateAsync(locaiton,cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return OperationResult<bool>.SuccessResult(true);
    }
}
