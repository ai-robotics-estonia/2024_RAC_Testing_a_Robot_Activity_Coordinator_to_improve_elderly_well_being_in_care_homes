using System.Security.Claims;

namespace Base.Helpers;

public static class IdentityExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        // External logins also provide the same claim! which is ours?
        var nameIdentifier = principal.FindFirst(ClaimTypes.NameIdentifier);

        return nameIdentifier == null ? Guid.Empty : Guid.Parse(nameIdentifier.Value);
    }

    public static string GetUserEmail(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);
        
        return principal.Identity?.Name ?? "null";
    }
}