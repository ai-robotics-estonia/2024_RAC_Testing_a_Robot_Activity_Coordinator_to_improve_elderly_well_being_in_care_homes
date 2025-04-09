using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.Identity.ViewModels;

public class ResetPasswordVM
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    public string? Url { get; set; }
}