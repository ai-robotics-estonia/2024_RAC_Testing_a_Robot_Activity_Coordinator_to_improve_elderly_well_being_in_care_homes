using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class AppVersion : BaseEntity
{
    public ulong ApkVersionCode { get; set; }
    
    [MaxLength(128)]
    public string ApkVersionName { get; set; } = default!;
   
    public DateTime UploadDT { get; set; }
    
    public Guid AppId { get; set; }
    public App? App { get; set; }
}