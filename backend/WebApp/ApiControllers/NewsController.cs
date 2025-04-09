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
public class NewsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<NewsController> _logger;
    private readonly DataService _dataService;

    public NewsController(AppDbContext context, IConfiguration configuration,
        ILogger<NewsController> logger, DataService dataService)
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
    [ProducesResponseType(typeof(List<NewsItemDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetNewsItems([FromQuery] string apiKey)
    {
        if (apiKey != _configuration.GetValue<string>("ApiKey"))
        {
            _logger.LogWarning("Unauth access");
            return BadRequest("Api key problem");
        }

        var newsItemsResult = await _dataService.GetErrNewsAsync();
        
        if (newsItemsResult.IsFailed) return BadRequest(newsItemsResult.Errors);
        var data = newsItemsResult.Value
            .Where(n => 
                !string.IsNullOrWhiteSpace(n.Description) && 
                n.Category != "ETV spordisaade" && 
                n.Category != "ETV uudised" && 
                n.Category != "Viipekeelsed" && 
                n.Category != "Raadiouudised")
            .ToList();
        return Ok(data);
    }

}