namespace App.DTO;

public class AppVersionDTO
{
    public ulong ApkVersionCode { get; set; }
    public string ApkVersionName { get; set; } = default!;

    public string Url { get; set; } = default!;
    public DateTime UploadDT { get; set; }
    public long FileSize { get; set; }
}