using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.Attendance;

public class LectureAttendance : BaseEntity
{
    public DateTime DT { get; set; } = DateTime.Now.ToUniversalTime();

    [MaxLength(128)]
    public string UserName { get; set; } = default!;

    public bool IsRegistration { get; set; }
    
    public Guid LectureId { get; set; }
    public Lecture? Lecture { get; set; }
}