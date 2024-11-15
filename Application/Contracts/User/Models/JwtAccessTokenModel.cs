namespace Application.Contracts.User.Models;

public record JwtAccessTokenModel(string AccessToken,int ExpirySecond, string TokenType="Bearer");
