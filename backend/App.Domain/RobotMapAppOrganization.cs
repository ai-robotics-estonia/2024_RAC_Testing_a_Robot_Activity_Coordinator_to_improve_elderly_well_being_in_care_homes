using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class RobotMapAppOrganization : BaseEntity
{
    [Display(Name = "Robot Map App")]
    public Guid RobotMapAppId { get; set; }
    public RobotMapApp? RobotMapApp { get; set; }

    [Display(Name = "Organization")]
    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }
}