namespace App.DTO;

public class NewsItemDTO
{
    public string Category { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public DateTime PubDate { get; set; }
    
    public string Link { get; set; } = default!;
    public string ShortLink { get; set; } = default!;
}