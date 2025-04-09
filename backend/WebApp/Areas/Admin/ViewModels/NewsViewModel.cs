using App.DTO;

namespace WebApp.Areas.Admin.ViewModels;

public class NewsViewModel
{
    public List<NewsItemDTO> NewsItems { get; set; } = default!;
}