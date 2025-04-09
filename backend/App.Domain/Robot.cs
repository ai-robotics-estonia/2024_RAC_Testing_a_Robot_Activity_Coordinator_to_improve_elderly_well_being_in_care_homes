using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;

[Index(nameof(AndroidIdCode), IsUnique = true)]
public class Robot : BaseEntity
{
    [MaxLength(255)] public string RobotName { get; set; } = default!;

    [MaxLength(255)] public string AndroidIdCode { get; set; } = default!;

    public ICollection<RobotMapApp>? RobotMapApps { get; set; }
}