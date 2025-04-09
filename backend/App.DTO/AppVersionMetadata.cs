namespace App.DTO;

public class AppVersionMetadata
{
  public string ApplicationId { get; set; } = default!;
  public List<Element> Elements { get; set; } = default!;
  
  public class Element
  {
    public ulong VersionCode { get; set; }
    public string VersionName { get; set; } = default!;
  }
}



