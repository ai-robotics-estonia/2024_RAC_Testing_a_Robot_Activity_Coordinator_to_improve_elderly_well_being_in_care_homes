using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class OrganizationAppUser : BaseEntity
{
    [Display(Name = "User")]
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    

    [Display(Name = "Organization")]
    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }
}

