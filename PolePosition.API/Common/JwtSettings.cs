namespace PolePosition.API.Common;

public class JwtSettings
{
    public string Key { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int ExpireMinutes { get; init; }

    public static JwtSettings FromEnvironment() => new()
    {
        Key = Environment.GetEnvironmentVariable("JWT_KEY")
            ?? throw new InvalidOperationException("JWT_KEY is not configured."),
        Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
            ?? throw new InvalidOperationException("JWT_ISSUER is not configured."),
        Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
            ?? throw new InvalidOperationException("JWT_AUDIENCE is not configured."),
        ExpireMinutes = int.TryParse(Environment.GetEnvironmentVariable("JWT_EXPIRE_MINUTES"), out var minutes)
            ? minutes
            : 60
    };
}