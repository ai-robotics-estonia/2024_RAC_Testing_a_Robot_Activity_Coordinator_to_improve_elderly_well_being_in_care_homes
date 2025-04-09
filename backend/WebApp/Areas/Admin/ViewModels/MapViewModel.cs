namespace WebApp.Areas.Admin.ViewModels;

public class MapViewModel
{
    public Guid Id { get; set; }
    public string MapName { get; set; } = default!;
    public string MapIdCode { get; set; } = default!;
    
    public int FloorCount { get; set; }
    public int LocationsCount { get; set; }
}