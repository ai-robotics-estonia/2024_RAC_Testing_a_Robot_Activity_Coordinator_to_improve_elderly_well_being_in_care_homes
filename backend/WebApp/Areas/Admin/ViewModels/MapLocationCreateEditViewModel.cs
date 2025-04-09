using App.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels;

public class MapLocationCreateEditViewModel
{
    public MapLocation MapLocation { get; set; } = default!;
    public string LocationDisplayName { get; set; } = default!;

    [ValidateNever]
    public SelectList MapFloorSelectList { get; set; } = default!;

} 