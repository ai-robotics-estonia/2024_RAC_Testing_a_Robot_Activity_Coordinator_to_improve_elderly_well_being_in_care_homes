using System.ComponentModel.DataAnnotations;

namespace App.DTO;

public class AttendanceDTO
{
    [MaxLength(128)]
    public string UserName { get; set; } = default!;

    public bool IsRegistration { get; set; } = default!;
    
    public Guid? LectureId { get; set; }
}