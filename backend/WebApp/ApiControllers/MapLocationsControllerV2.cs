using System.Net;
using App.BLL;
using App.DAL.EF;
using App.DTO;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/MapLocations")]
public class MapLocationsControllerV2 : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MapLocationsControllerV2> _logger;
    private readonly DataService _dataService;

    public MapLocationsControllerV2(AppDbContext context, IConfiguration configuration,
        ILogger<MapLocationsControllerV2> logger, DataService dataService)
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
        [FromBody]
        MapDTO mapDTO,
        [FromQuery]
        string apiKey)
    {
        if (apiKey != _configuration.GetValue<string>("ApiKey"))
        {
            _logger.LogWarning("Unauth access");
            return BadRequest("Api key problem");
        }
 
        
        var res = await _dataService.UpdateMapAsync(mapDTO);

        if (res.IsFailed)
        {
            return BadRequest(res.Errors);
        }

        return Ok(res.Value);
    }

    [HttpGet("FixFloors")]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> FixFloors(
        [FromQuery]
        string apiKey)
    {
        if (apiKey != _configuration.GetValue<string>("ApiKey"))
        {
            _logger.LogWarning("Unauth access");
            return BadRequest("Api key problem");
        }

        
        var res = await _dataService.FixMissingFloorsAsync();

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
    [ProducesResponseType(typeof(List<MapLocationSync2DTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetMapLocations(string mapIdCode, [FromQuery] string apiKey)
    {
        if (apiKey != _configuration.GetValue<string>("ApiKey"))
        {
            _logger.LogWarning("Unauth access");
            return BadRequest("Api key problem");
        }

        var map = await _context.Maps
            .Include(m => m.MapFloors!)
            .ThenInclude(mf => mf.MapLocations)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.MapIdCode == mapIdCode);
        
        if (map is null)
        {
            return NotFound();
        }


        var res = new List<MapLocationSync2DTO>();

        foreach (var mapFloor in map.MapFloors!)
        {
            foreach (var mapLocation in mapFloor.MapLocations!)
            {
                var dto = new MapLocationSync2DTO()
                {
                    FloorName = mapFloor.FloorName,
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
        }

        return Ok(res);
    }


}