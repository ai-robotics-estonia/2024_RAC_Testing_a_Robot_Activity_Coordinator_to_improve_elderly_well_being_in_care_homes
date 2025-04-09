using System.ComponentModel.DataAnnotations;

namespace App.DTO;

public class WebLinkDTO
{
    public Guid Id { get; set; }
    
    [MaxLength(1024)]
    public string Uri { get; set; } = default!;

    public bool IsIframe { get; set; }

    public float? ZoomFactor { get; set; }
    public int? TextZoom { get; set; }
    public bool LoadWithOverviewMode { get; set; }
    public bool UseWideViewPort { get; set; }
    public string LayoutAlgorithm { get; set; } = default!;
    public bool BuiltInZoomControls { get; set; }
    public bool DisplayZoomControls { get; set; }

    
    [MaxLength(128)]
    public string WebLinkName { get; set; } = default!;
    
    [MaxLength(128)]
    public string WebLinkDisplayName { get; set; } = default!;
}