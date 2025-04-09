using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;

[Index(nameof(MapIdCode), IsUnique = true)]
public class Map : BaseEntity
{
    [MaxLength(255)]
    public string MapName { get; set; } = default!;

    [MaxLength(255)]
    public string MapIdCode { get; set; } = default!;


    [Display(Name = "Map Floors")]
    public ICollection<MapFloor>? MapFloors { get; set; }

    [Display(Name = "Map Locations")]
    public ICollection<MapLocation>? MapLocations { get; set; }

    public ICollection<RobotMapApp>? RobotMapApps { get; set; }
}