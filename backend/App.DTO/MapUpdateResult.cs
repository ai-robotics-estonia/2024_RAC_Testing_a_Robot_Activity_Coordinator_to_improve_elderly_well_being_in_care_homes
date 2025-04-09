namespace App.DTO;

public class MapUpdateResult
{
    public int FloorsAdded { get; set; }
    public int FloorsRemoved { get; set; }
    public int LocationsAdded { get; set; }
    public int LocationsRemoved { get; set; }
    public int LocationsFixed { get; set; }
}
