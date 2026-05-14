namespace HermanosDeLeche.Domain.Configuration;

public sealed class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = "HermanosDeLeche";
    public string Audience { get; set; } = "HermanosDeLeche";
    public int ExpiryMinutes { get; set; } = 10080;
}
