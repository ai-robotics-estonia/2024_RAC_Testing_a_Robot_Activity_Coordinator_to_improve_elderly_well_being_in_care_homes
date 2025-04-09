using App.Domain;

namespace WebApp.Areas.Admin.ViewModels;

public class AppVersionsIndexViewModel
{
    public List<VersionDetailInfo> VersionDetailInfos { get; set; } = default!;
}

public class VersionDetailInfo
{
    public AppVersion AppVersion { get; set; } = default!;
    public long? FileSize { get; set; }
    public DateTime? DT { get; set; }
}