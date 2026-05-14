using HermanosDeLeche.Domain.DTOs.Auth;
using HermanosDeLeche.Domain.Entities;
using HermanosDeLeche.Domain.Exceptions;
using HermanosDeLeche.Domain.Interfaces;
using HermanosDeLeche.Service.Security;

namespace HermanosDeLeche.Service.Services;

public sealed class AuthService : IAuthService
{
    private readonly IMilkmanRepository _milkmen;
    private readonly JwtTokenIssuer _tokens;

    public AuthService(IMilkmanRepository milkmen, JwtTokenIssuer tokens)
    {
        _milkmen = milkmen;
        _tokens = tokens;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        if (await _milkmen.GetByUsernameAsync(request.Username.Trim(), ct) is not null)
            throw new AppException("El nombre de usuario ya está en uso.");

        if (await _milkmen.GetByEmailAsync(request.Email.Trim(), ct) is not null)
            throw new AppException("El correo ya está registrado.");

        var milkman = new Milkman
        {
            Nombre = request.Nombre.Trim(),
            Username = request.Username.Trim(),
            Email = request.Email.Trim().ToLowerInvariant(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Ciudad = request.Ciudad?.Trim(),
            FotoUrl = request.FotoUrl?.Trim()
        };

        await _milkmen.CreateAsync(milkman, ct);

        return new AuthResponse
        {
            Token = _tokens.CreateToken(milkman.Id, milkman.Username, milkman.Email),
            MilkmanId = milkman.Id,
            Username = milkman.Username,
            Email = milkman.Email
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var milkman = await _milkmen.GetByUsernameAsync(request.Username.Trim(), ct)
            ?? throw new AppException("Usuario o contraseña inválidos.", 401);

        if (!BCrypt.Net.BCrypt.Verify(request.Password, milkman.PasswordHash))
            throw new AppException("Usuario o contraseña inválidos.", 401);

        return new AuthResponse
        {
            Token = _tokens.CreateToken(milkman.Id, milkman.Username, milkman.Email),
            MilkmanId = milkman.Id,
            Username = milkman.Username,
            Email = milkman.Email
        };
    }
}
