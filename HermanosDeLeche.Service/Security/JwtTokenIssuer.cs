using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HermanosDeLeche.Domain.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HermanosDeLeche.Service.Security;

public sealed class JwtTokenIssuer
{
    private readonly JwtSettings _settings;

    public JwtTokenIssuer(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }

    public string CreateToken(Guid milkmanId, string username, string email)
    {
        if (string.IsNullOrWhiteSpace(_settings.Key) || _settings.Key.Length < 32)
            throw new InvalidOperationException("JWT Key must be at least 32 characters.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, milkmanId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
