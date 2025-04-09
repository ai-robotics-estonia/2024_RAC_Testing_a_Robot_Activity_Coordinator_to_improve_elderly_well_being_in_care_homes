using System.Net;
using App.BLL;
using App.DAL.EF;
using App.Domain;
using App.DTO;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class MapLocationsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MapLocationsController> _logger;
    private readonly DataService _dataService;

    public MapLocationsController(AppDbContext context, IConfiguration configuration,
        ILogger<MapLocationsController> logger, DataService dataService)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
        _dataService = dataService;
    }


    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> PostMapLocation(
        [FromBody] MapLocationsDTO mapLocationsDTO,
        [FromQuery] string apiKey)
    {
        if (apiKey != _configuration.GetValue<string>("ApiKey"))
        {
            _logger.LogWarning("Unauth access");
            return BadRequest("Api key problem");
        }

        var res = await _dataService.UpdateMapLocationAsync(mapLocationsDTO);

        if (res.IsFailed)
        {
            return BadRequest(res.Errors);
        }

        return Ok(res.Value);
    }


    [HttpGet("{mapIdCode}")]
    [Consumes("application/json")]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(List<MapLocationSyncDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetMapLocations(string mapIdCode, [FromQuery] string apiKey)
    {
        if (apiKey != _configuration.GetValue<string>("ApiKey"))
        {
            _logger.LogWarning("Unauth access");
            return BadRequest("Api key problem");
        }
        
        var map = await _context.Maps.Include(m => m.MapLocations).FirstOrDefaultAsync(m => m.MapIdCode == mapIdCode);
        if (map is null)
        {
            return NotFound();
        }

        var res = new List<MapLocationSyncDTO>();

        foreach (var mapLocation in map.MapLocations!)
        {
            var dto = new MapLocationSyncDTO()
            {
                MapLocation = mapLocation.LocationName,
                SortPriority = mapLocation.SortPriority,
                PatrolPriority = mapLocation.PatrolPriority,
                Translations = []
            };

            foreach (var displayName in mapLocation.LocationDisplayName)
            {
                dto.Translations.Add(new TranslationDTO()
                {
                    Lang = displayName.Key,
                    Value = displayName.Value
                });
            }
            res.Add(dto);
        }

        return Ok(res);
    }
}