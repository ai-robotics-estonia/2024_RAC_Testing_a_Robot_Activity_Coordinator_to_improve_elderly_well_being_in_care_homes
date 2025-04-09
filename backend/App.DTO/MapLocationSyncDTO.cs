namespace App.DTO;

public class MapLocationSyncDTO
{
    
    public string MapLocation { get; set; } = default!;

    public int SortPriority { get; set; }
    public int PatrolPriority { get; set; }
    public List<TranslationDTO> Translations { get; set; } = default!;
}