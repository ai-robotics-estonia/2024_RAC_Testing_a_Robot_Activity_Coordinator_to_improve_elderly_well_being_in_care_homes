using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class MapFloor : BaseEntity
{
    [MaxLength(128)]
    public string FloorName { get; set; } = default!;   
    
    [Column(TypeName = "jsonb")]
    public LangStr FloorDisplayName { get; set; } = default!;
    
    [Display(Name = "Map")]
    public Guid MapId { get; set; }

    public Map? Map { get; set; }

    [Display(Name = "Map Locations")]
    public ICollection<MapLocation>? MapLocations { get; set; }
}