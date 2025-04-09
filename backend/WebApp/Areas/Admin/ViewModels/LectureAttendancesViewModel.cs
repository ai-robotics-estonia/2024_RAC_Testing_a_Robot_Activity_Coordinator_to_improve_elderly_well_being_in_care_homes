using System.Globalization;
using Base.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels;

public class LectureAttendancesViewModel
{
    [ValidateNever]
    public X.PagedList.IPagedList<App.Domain.Attendance.LectureAttendance> LectureAttendances { get; set; } = default!;

    public string CreatedAtFrom { get; set; } = DateTime.Now.BeginningOfToday().ToLocalTime().ToString(CultureInfo.CurrentCulture);
    
    public string CreatedAtTo { get; set; } = DateTime.Now.EndOfToday().ToLocalTime().ToString(CultureInfo.CurrentCulture);

    [ValidateNever]
    public SelectList PageSizeSelectList { get; set; } = default!;

    public int PageSize { get; set; } = 100;

    [ValidateNever]
    public SelectList LectureSelectList { get; set; } = default!;

    public Guid? LectureId { get; set; }


    [ValidateNever]
    public SelectList EventTypeSelectList { get; set; } = default!;

    public bool? EventType { get; set; }


    public string? SubmitAction { get; set; }
}