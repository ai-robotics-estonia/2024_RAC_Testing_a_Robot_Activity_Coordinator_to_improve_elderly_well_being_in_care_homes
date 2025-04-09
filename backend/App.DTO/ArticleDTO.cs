using System.ComponentModel.DataAnnotations;

namespace App.DTO;

public class ArticleDTO
{
    [MaxLength(128)]
    public string Title { get; set; } = default!;

    public string DisplayTitle { get; set; } = default!;

    public string PlainText { get; set; } = default!;

    public string DisplayText { get; set; } = default!;
}