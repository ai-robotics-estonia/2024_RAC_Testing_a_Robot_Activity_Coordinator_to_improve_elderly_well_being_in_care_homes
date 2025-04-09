namespace WebApp.Areas.Admin.ViewModels;

public class RobotMapAppViewModel
{
    public Guid Id { get; set; }
    public string RobotName { get; set; } = default!;
    public string MapName { get; set; } = default!;
    public string AppName { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public int LogEventsCount { get; set; }
    public int ArticlesCount { get; set; }
}