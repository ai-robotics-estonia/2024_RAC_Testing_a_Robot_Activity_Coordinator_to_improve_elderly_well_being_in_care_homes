using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;

[Index(nameof(RobotId), nameof(MapId), nameof(AppId), IsUnique = true)]
public class RobotMapApp : BaseEntity
{
    [Display(Name = "Robot")]
    public Guid RobotId { get; set; }
    public Robot? Robot { get; set; }

    [Display(Name = "Map")]
    public Guid MapId  { get; set; }
    public Map? Map { get; set; }

    [Display(Name = "App")]
    public Guid AppId { get; set; }
    public App? App { get; set; }

    [MaxLength(128)]
    public string DisplayName { get; set; } = default!;

    public ICollection<RobotMapAppOrganization>? RobotMapAppOrganizations { get; set; }

    public ICollection<Article>? Articles { get; set; }

    public ICollection<LogEvent>? LogEvents { get; set; }
}

