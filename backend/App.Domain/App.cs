using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;

[Index(nameof(AppName), IsUnique = true)]
public class App : BaseEntity
{
    [MaxLength(128)]
    public string AppName { get; set; } = default!;
    
    public ICollection<RobotMapApp>? RobotMapApps { get; set; }

    public ICollection<AppVersion>? AppVersions { get; set; }
}