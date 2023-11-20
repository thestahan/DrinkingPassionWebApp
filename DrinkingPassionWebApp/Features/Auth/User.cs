using System.Security.Claims;

namespace DrinkingPassionWebApp.Features.Auth;

public class User
{
    public string Email { get; set; } = default!;
    public string? DisplayName { get; set; }

    public ICollection<string> Roles { get; set; } =
        new List<string>();

    public ClaimsPrincipal ToClaimsPrincipal()
    {
        var rolesClaims = Roles.Select(r => new Claim(ClaimTypes.Role, r)).ToArray();
        var claims = new Claim[]
        {
            new(ClaimTypes.Email, Email),
            new(ClaimTypes.GivenName, DisplayName ?? string.Empty)
        }.Concat(rolesClaims);

        var claimsIdentity = new ClaimsIdentity(claims, AuthConstants.AuthenticationType);

        return new ClaimsPrincipal(claimsIdentity);
    }

    public static User FromClaimsPrincipal(ClaimsPrincipal principal)
    {
        return new()
        {
            Email = principal.FindFirst("email")?.Value ?? "",
            DisplayName = principal.FindFirst("given_name")?.Value ?? "",
            Roles = principal.FindAll("role").Select(c => c.Value).ToList()
        };
    }
}