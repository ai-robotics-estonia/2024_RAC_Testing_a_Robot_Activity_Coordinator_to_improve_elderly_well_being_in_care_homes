using System.ComponentModel.DataAnnotations;
using App.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels;

public class WebLinkViewModel
{
    public WebLink WebLink { get; set; } = default!;

    [MaxLength(128)]
    public string WebLinkDisplayName { get; set; } = default!;
    
    [ValidateNever]
    public SelectList OrganizationSelectList { get; set; } = default!;
    
    [ValidateNever]
    public SelectList WebLinkCategorySelectList { get; set; } = default!;
}