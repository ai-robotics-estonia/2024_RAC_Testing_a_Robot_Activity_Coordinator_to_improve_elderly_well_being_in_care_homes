using App.Domain;
using Base.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels;

public class LogEventsViewModel
{
    [ValidateNever]
    public X.PagedList.IPagedList<App.Domain.LogEvent> LogEvents { get; set; } = default!;

    public string? Tag { get; set; } = default!;
    public string? Message { get; set; } = default!;
    public DateTime CreatedAtFrom { get; set; } = DateTime.Now.FirstDayOfMonth();
    public DateTime CreatedAtTo { get; set; } = DateTime.Now.LastDayWithTimeOfMonth();

    [ValidateNever]
    public SelectList PageSizeSelectList { get; set; } = default!;
    public int PageSize { get; set; } = 100;
    
    
    [ValidateNever]
    public SelectList RobotSelectList { get; set; } = default!;
    public Guid? RobotId { get; set; }
    
    [ValidateNever]
    public SelectList AppSelectList { get; set; } = default!;
    public Guid? AppId { get; set; }
    
    [ValidateNever]
    public SelectList MapSelectList { get; set; } = default!;
    public Guid? MapId { get; set; }
    
}