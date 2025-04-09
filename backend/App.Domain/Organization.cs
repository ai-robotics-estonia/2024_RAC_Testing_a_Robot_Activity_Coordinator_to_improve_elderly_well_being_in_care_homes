using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;

[Index(nameof(OrgName), IsUnique = true)]
public class Organization : BaseEntity
{
    [MaxLength(128)]
    public string OrgName { get; set; } = default!;

    public ICollection<OrganizationAppUser>? OrganizationAppUsers { get; set; }

    public ICollection<RobotMapAppOrganization>? RobotMapAppOrganizations { get; set; }
}