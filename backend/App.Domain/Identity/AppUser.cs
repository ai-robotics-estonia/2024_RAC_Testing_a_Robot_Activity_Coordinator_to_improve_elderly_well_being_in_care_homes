using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity;

public class AppUser : IdentityUser<Guid>
{
    public ICollection<OrganizationAppUser>? OrganizationAppUsers { get; set; }
}