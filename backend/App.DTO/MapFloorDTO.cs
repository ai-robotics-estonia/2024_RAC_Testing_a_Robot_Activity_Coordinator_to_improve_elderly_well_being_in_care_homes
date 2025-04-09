using System.ComponentModel.DataAnnotations;
using System.Security;

namespace App.DTO;

public class MapFloorDTO
{
    [MaxLength(128)]
    public string FloorName { get; set; } = default!;
    
    public List<string> MapLocations { get; set; } = default!;
}