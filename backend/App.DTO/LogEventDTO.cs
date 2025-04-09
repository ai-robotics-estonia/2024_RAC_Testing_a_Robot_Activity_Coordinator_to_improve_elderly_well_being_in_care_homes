using System.ComponentModel.DataAnnotations;

namespace App.DTO;

public class LogEventDTO
{

    [MaxLength(128)]
    public string AndroidIdCode { get; set; } = default!;

    [MaxLength(128)]
    public string AppLaunch { get; set; } = default!;
    
    [MaxLength(128)]
    public string MapIdCode { get; set; } = default!;

    [MaxLength(256)]
    public string MapName { get; set; } = default!;

    [MaxLength(128)]
    public string AppName { get; set; } = default!;

    [MaxLength(128)]
    public string Tag { get; set; } = default!;

    [MaxLength(512)]
    public string? Message { get; set; }
    
    public int? IntValue { get; set; }
   
    public double? DoubleValue { get; set; }
}