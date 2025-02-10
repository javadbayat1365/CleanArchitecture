using Application.Common;
using Application.Repositories.Common;
using Mediator;

namespace Application.Features.Location.Queries.GetLocationsByName;

public sealed class GetLocationsByNameQueryHandler(IUnitOfWork unitOfWork) 
        : IRequestHandler<GetLocationsByNameQuery, OperationResult<IEnumerable<GetLocationsByNameQueryResult>>>
{
    public async ValueTask<OperationResult<IEnumerable<GetLocationsByNameQueryResult>>> Handle(GetLocationsByNameQuery request, CancellationToken cancellationToken)
    {
        var locations =await  unitOfWork.LocationRepository.GetLocationsByNameAsync(request.LocationName,cancellationToken);

        if (!locations.Any())
            return OperationResult<IEnumerable<GetLocationsByNameQueryResult>>
                             .SuccessResult(Enumerable.Empty<GetLocationsByNameQueryResult>().ToList());

        return OperationResult<IEnumerable<GetLocationsByNameQueryResult>>.SuccessResult(
            locations.Select(s => new GetLocationsByNameQueryResult(s.Id,s.Name)));
    }
}
