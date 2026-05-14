using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using HermanosDeLeche.Domain.Exceptions;

namespace HermanosDeLeche.Api.Security;

public static class HttpUserExtensions
{
    private const string MsftNameId = "http://schemas.microsoft.com/ws/2008/06/identity/claims/nameidentifier";

    public static Guid GetMilkmanId(this ClaimsPrincipal user)
    {
        var sub = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
            ?? user.FindFirst("sub")?.Value
            ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? user.FindFirst(MsftNameId)?.Value;

        if (string.IsNullOrEmpty(sub))
            throw new AppException("Token sin identidad de lechero.", 401);

        if (!Guid.TryParse(sub, out var id))
            throw new AppException("Token inválido.", 401);

        return id;
    }
}
