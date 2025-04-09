using System.Runtime.InteropServices.JavaScript;

namespace WebApp.Areas.Admin.ViewModels;

public class LectureAttendancesCsv
{
    public DateTime DT { get; set; }
    public string UserName { get; set; } = default!;
    public string LectureName { get; set; } = default!;
    public bool IsRegistration { get; set; }
    
}