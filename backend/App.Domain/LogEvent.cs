using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;


[Index(nameof(Tag), nameof(CreatedAt))]
public class LogEvent : BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    [MaxLength(128)]
    public string AppLaunch { get; set; } = default!;

    [MaxLength(128)]
    public string Tag { get; set; } = default!;

    [MaxLength(512)]
    public string? Message { get; set; }

    public int? IntValue { get; set; }
    
    public double? DoubleValue { get; set; }

    [Display(Name = "Robot Map App")]
    public Guid RobotMapAppId { get; set; }
    public RobotMapApp? RobotMapApp { get; set; }
}
