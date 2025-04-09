namespace App.DTO;

public class MapLocationSync2DTO
{
    public string FloorName { get; set; } = default!;
    
    public string MapLocation { get; set; } = default!;

    public int SortPriority { get; set; }
    public int PatrolPriority { get; set; }
    
    
    public List<TranslationDTO> Translations { get; set; } = default!;
}