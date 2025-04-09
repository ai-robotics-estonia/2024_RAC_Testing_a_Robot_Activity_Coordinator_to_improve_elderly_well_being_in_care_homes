using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;

[Index(nameof(LocationName), nameof(MapFloorId), IsUnique = true)]
public class MapLocation : BaseEntity
{
    [MaxLength(128)]
    public string LocationName { get; set; } = default!;

    [Column(TypeName = "jsonb")]
    public LangStr LocationDisplayName { get; set; } = default!;

    [Display(Name = "Sort priority")]
    public int SortPriority { get; set; }

    [Display(Name = "Patrol priority (0 - exclude from patrol)")]
    public int PatrolPriority { get; set; }

    [Display(Name = "Map")]
    public Guid MapId { get; set; }

    public Map? Map { get; set; }

    [Display(Name = "Floor")]
    public Guid? MapFloorId { get; set; }
    public MapFloor? MapFloor { get; set; }
}