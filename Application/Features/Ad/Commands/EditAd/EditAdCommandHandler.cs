using Application.Common;
using Application.Contracts.FileService.Interfaces;
using Application.Repositories.Common;
using Mediator;

namespace Application.Features.Ad.Commands.EditAd;

public class EditAdCommandHandler(IUnitOfWork unitOfWork,IFileService fileService) : IRequestHandler<EditAdCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(EditAdCommand request, CancellationToken cancellationToken)
    {
         

        return OperationResult<bool>.SuccessResult(true);
    }
}
 