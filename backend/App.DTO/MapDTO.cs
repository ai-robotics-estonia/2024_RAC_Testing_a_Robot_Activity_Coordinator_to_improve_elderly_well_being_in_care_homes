using System.ComponentModel.DataAnnotations;

namespace App.DTO;

public class MapDTO
{
    [MaxLength(128)]
    public string MapIdCode { get; set; } = default!;

    [MaxLength(256)]
    public string MapName { get; set; } = default!;

    public List<MapFloorDTO> MapFloors { get; set; } = default!;
}