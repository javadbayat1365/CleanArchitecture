using Application.Common;
using Application.Repositories.Common;
using Mediator;

namespace Application.Features.Location.Queries.GetLocationById;

public sealed class GetLocationByIdQueryHandler(IUnitOfWork unitOfWork) 
      : IRequestHandler<GetLocationByIdQuery, OperationResult<GetLocationByIdQueryResult>>
{
    public async ValueTask<OperationResult<GetLocationByIdQueryResult>> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
    {
        var location = await unitOfWork.LocationRepository.GetLocationByIdAsync(request.LocationId,cancellationToken);
        if (location == null)
            return OperationResult<GetLocationByIdQueryResult>.FailureResult(nameof(request.LocationId));

        return OperationResult<GetLocationByIdQueryResult>
            .SuccessResult(new GetLocationByIdQueryResult(location.Id,location.Name));
    }
}
