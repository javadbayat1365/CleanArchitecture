namespace Application.Contracts.User.Models;

public record JwtAccessTokenModel(string AccessToken,double ExpirySecond, string TokenType="Bearer");
