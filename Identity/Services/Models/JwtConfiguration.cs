namespace Identity.Services.Models;

internal class JwtConfiguration
{
    public string SignInKey { get; set; } = default!;//must be at least 16 chars
    public string Audience { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public int ExpirationMinute { get; set; }
    public string EncryptionKey { get; set; } = default!;//must be 16 chars
}
