using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class WebLink : BaseEntity
{
    [MaxLength(1024)]
    public string Uri { get; set; } = default!;

    public bool IsIframe { get; set; }  = true;

    public float? ZoomFactor { get; set; }
    public int? TextZoom { get; set; }

    public bool LoadWithOverviewMode { get; set; } = true;
    public bool UseWideViewPort { get; set; }  = true;
    public EWebViewLayoutAlgorithm LayoutAlgorithm { get; set; } = EWebViewLayoutAlgorithm.TEXT_AUTOSIZING;
    public bool BuiltInZoomControls { get; set; }  = true;
    public bool DisplayZoomControls { get; set; }  = true;

    [MaxLength(128)]
    public string WebLinkName { get; set; } = default!;
    
    [Column(TypeName = "jsonb")]
    public LangStr WebLinkDisplayName { get; set; } = default!;

    public Guid WebLinkCategoryId { get; set; }
    public WebLinkCategory? WebLinkCategory { get; set; }
    
    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }
}