using Application.Contracts.User.Models;
using Domain.Entities.User;

namespace Application.Contracts.User;

public interface IJwtService
{
    Task<JwtAccessTokenModel> GetJwtAccessTokenAsync(UserEntity user,CancellationToken cancellationToken);
}
