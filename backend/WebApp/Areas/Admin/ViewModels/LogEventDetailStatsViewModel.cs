using Base.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebApp.Areas.Admin.ViewModels;

public class LogEventDetailStatsViewModel
{
    [ValidateNever]
    public List<StatItem> Stats { get; set; } = default!;

    public DateTime CreatedAtFrom { get; set; } = DateTime.Now.FirstDayOfMonth();
    public DateTime CreatedAtTo { get; set; } = DateTime.Now.LastDayWithTimeOfMonth();

    public string RobotName { get; set; } = default!;
    public string MapName { get; set; } = default!;
    public string AppName { get; set; } = default!;

    public string Key { get; set; } = default!;
}