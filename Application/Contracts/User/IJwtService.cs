using Application.Contracts.User.Models;
using Domain.Entities.User;

namespace Application.Contracts.User;

public interface IJwtService
{
    Task<JwtAccessTokenModel> GenerateTokenAsync(UserEntity user,CancellationToken cancellationToken);
}
