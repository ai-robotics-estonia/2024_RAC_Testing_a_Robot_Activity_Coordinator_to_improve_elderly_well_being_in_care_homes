using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.Attendance;

public class Lecture : BaseEntity
{
    [MaxLength(128)]
    public string Name { get; set; } = default!;

    public ICollection<LectureAttendance>? LectureAttendances { get; set; }
}