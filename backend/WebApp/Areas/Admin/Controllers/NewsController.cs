using System.Xml.Linq;
using App.BLL;
using App.DAL.EF;
using App.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using WebApp.Areas.Admin.ViewModels;

namespace WebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin")]
public class NewsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<NewsController> _logger;
    private readonly DataService _dataService;


    public NewsController(AppDbContext context, IHttpClientFactory httpClientFactory, DataService dataService,
        ILogger<NewsController> logger)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _dataService = dataService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var newsResult = await _dataService.GetErrNewsAsync();

        var vm = new NewsViewModel();
        if (newsResult.IsFailed)
        {
            vm.NewsItems = new List<NewsItemDTO>()
            {
                new NewsItemDTO()
                {
                    Category = "ERROR",
                    Title = "Error",
                    Description = newsResult.Errors.ToString()!,
                    Link = "",
                    ShortLink = "",
                    PubDate = DateTime.Now,
                }
            };
        }

        vm.NewsItems = newsResult.Value;

        return View(vm);
    }
}