using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using HermanosDeLeche.Domain.Exceptions;

namespace HermanosDeLeche.Api.Security;

public static class HttpUserExtensions
{
    private const string MsftNameId = "http://schemas.microsoft.com/ws/2008/06/identity/claims/nameidentifier";

    public static Guid GetMilkmanId(this ClaimsPrincipal user)
    {
        foreach (var claim in user.Identities.SelectMany(static i => i.Claims))
        {
            if (IsSubClaimType(claim.Type) && Guid.TryParse(claim.Value, out var id))
                return id;
        }

        throw new AppException("Token sin identidad de lechero.", 401);
    }

    private static bool IsSubClaimType(string type) =>
        type == JwtRegisteredClaimNames.Sub
        || type == "sub"
        || type == ClaimTypes.NameIdentifier
        || type == MsftNameId;
}
