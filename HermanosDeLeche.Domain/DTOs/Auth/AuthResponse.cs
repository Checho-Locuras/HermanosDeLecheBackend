namespace HermanosDeLeche.Domain.DTOs.Auth;

public sealed class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public Guid MilkmanId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
