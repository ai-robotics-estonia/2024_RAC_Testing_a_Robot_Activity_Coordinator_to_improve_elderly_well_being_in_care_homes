using System.Net;
using App.BLL;
using App.DAL.EF;
using App.Domain;
using App.DTO;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApp.Areas.Admin.ViewModels;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ArticlesController> _logger;
    private readonly DataService _dataService;

    public ArticlesController(AppDbContext context, IConfiguration configuration,
        ILogger<ArticlesController> logger, DataService dataService)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
        _dataService = dataService;
    }


    [HttpGet]
    [Consumes("application/json")]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ArticleDTO), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetArticle(
        [FromQuery] string articleTitle,
        [FromQuery] string androidIdCode,
        [FromQuery] string mapIdCode, [FromQuery] string mapName, [FromQuery] string appName,
        [FromQuery] string apiKey,
        [FromQuery] DateOnly? date = null
    )
    {
        if (apiKey != _configuration.GetValue<string>("ApiKey"))
        {
            _logger.LogWarning("Unauth access");
            return BadRequest("Api key problem");
        }

        var robotMapAppId =
            await _dataService.GetOrCreateRobotMapAppIdAsync(androidIdCode, mapIdCode, mapName, appName);


        articleTitle = articleTitle.ToUpper();
        
        Article? article = null;

        
        if (date != null)
        {
            article =
                await _context.Articles
                    .Include(a => a.ArticleCategory)
                    .FirstOrDefaultAsync(a =>
                        a.RobotMapAppId == robotMapAppId &&
                        a.Date == date &&
                        a.ArticleCategory!.CategoryName.ToUpper().Equals(articleTitle)
                    );
        }

        // get the default article
        article ??= await _context.Articles
            .Include(a => a.ArticleCategory)
            .FirstOrDefaultAsync(a =>
                a.RobotMapAppId == robotMapAppId &&
                a.Date == null &&
                a.ArticleCategory!.CategoryName.ToUpper().Equals(articleTitle)
            );


        if (article == null)
        {
            return NotFound();
        }

        var res = new ArticleDTO()
        {
            Title = article.ArticleCategory!.CategoryName,
            DisplayTitle = article.ArticleCategory.CategoryDisplayName,
            PlainText = article.PlainText,
            DisplayText = article.DisplayText
        };

        return Ok(res);
    }
}