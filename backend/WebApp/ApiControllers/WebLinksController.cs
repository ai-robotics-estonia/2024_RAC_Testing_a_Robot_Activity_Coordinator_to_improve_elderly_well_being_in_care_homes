using System.Net;
using App.BLL;
using App.DAL.EF;
using App.DTO;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class WebLinksController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<WebLinksController> _logger;
    private readonly DataService _dataService;

    public WebLinksController(AppDbContext context, IConfiguration configuration,
        ILogger<WebLinksController> logger, DataService dataService)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
        _dataService = dataService;
    }

    [HttpGet("{categoryName}")]
    [Consumes("application/json")]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(List<WebLinkDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<WebLinkDTO>>> GetWebLink(
        [FromQuery]
        string androidIdCode,
        [FromQuery]
        string mapIdCode,
        [FromQuery]
        string mapName,
        [FromQuery]
        string appName,
        [FromQuery]
        string apiKey,
        string categoryName
    )
    {
        if (apiKey != _configuration.GetValue<string>("ApiKey"))
        {
            _logger.LogWarning("Unauth access");
            return BadRequest("Api key problem");
        }

        var robotMapAppId =
            await _dataService.GetOrCreateRobotMapAppIdAsync(androidIdCode, mapIdCode, mapName, appName);

        var organizationId =
            (await _context.RobotMapAppOrganizations.FirstOrDefaultAsync(r => r.RobotMapAppId == robotMapAppId))
            ?.OrganizationId;

        if (organizationId == null)
        {
            return NotFound("Organization not found");
        }

        categoryName = categoryName.ToUpper();
        var webLinks = await _context.WebLinks
            .Where(w =>
                w.WebLinkCategory!.CategoryName.ToUpper() == categoryName &&
                w.OrganizationId == organizationId
            )
            .ToListAsync();
        var res = webLinks.Select(w => new WebLinkDTO()
            {
                Id = w.Id,
                Uri = w.Uri,
                IsIframe = w.IsIframe,
                ZoomFactor = w.ZoomFactor,
                TextZoom = w.TextZoom,
                LoadWithOverviewMode = w.LoadWithOverviewMode,
                UseWideViewPort = w.UseWideViewPort,
                LayoutAlgorithm = w.LayoutAlgorithm.ToString(),
                BuiltInZoomControls = w.BuiltInZoomControls,
                DisplayZoomControls = w.DisplayZoomControls,

                WebLinkName = w.WebLinkName,
                WebLinkDisplayName = w.WebLinkDisplayName.ToString()
            })
            .OrderBy(w => w.WebLinkDisplayName)
            .ToList();

        return Ok(res);
    }
}