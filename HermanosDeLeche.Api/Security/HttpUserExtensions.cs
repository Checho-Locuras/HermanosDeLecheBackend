using System.Security.Claims;

using HermanosDeLeche.Domain.Exceptions;

namespace HermanosDeLeche.Api.Security;

public static class HttpUserExtensions
{
    public static Guid GetMilkmanId(this ClaimsPrincipal user)
    {
        var sub = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? user.FindFirst("sub")?.Value
            ?? throw new AppException("Token sin identidad de lechero.", 401);

        if (!Guid.TryParse(sub, out var id))
            throw new AppException("Token inválido.", 401);

        return id;
    }
}
