using System.Net;
using App.BLL;
using App.DAL.EF;
using App.Domain;
using App.DTO;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class LogEventsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<LogEventsController> _logger;
    private readonly DataService _dataService;

    public LogEventsController(AppDbContext context, IConfiguration configuration, ILogger<LogEventsController> logger, DataService dataService)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
        _dataService = dataService;
    }

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType((int) HttpStatusCode.Created )]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<ActionResult> PostLogEvent([FromBody] LogEventDTO logEvent, [FromQuery] string apiKey)
    {
        if (apiKey != _configuration.GetValue<string>("ApiKey"))
        {
            _logger.LogWarning("Unauth access");
            return BadRequest("Api key problem");
        }
        

        var logEventDomain = await _dataService.MapLogEventAsync(logEvent);
        

        _context.LogEvents.Add(logEventDomain);
        await _context.SaveChangesAsync();

        return Created("", new { id = logEventDomain.Id });
        // CreatedAtAction("GetLogEvent", new { id = logEvent.Id }, logEvent);
    }

    
    
    /*

    // POST: api/LogEvents
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType((int) HttpStatusCode.Created )]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<ActionResult> PostLogEvent([FromBody] LogEventDTO logEvent, [FromQuery] string apiKey)
    {
        if (apiKey != _configuration.GetValue<string>("ApiKey"))
        {
            _logger.LogWarning("Unauth access");
            return BadRequest("Api key problem");
        }

        var logEventDomain = logEvent.ToDomainLogEvent();

        _context.LogEvents.Add(logEventDomain);
        await _context.SaveChangesAsync();

        return Created("", new { id = logEventDomain.Id });
        // CreatedAtAction("GetLogEvent", new { id = logEvent.Id }, logEvent);
    }
    */


    // GET: api/LogEvents
    /*
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LogEvent>>> GetLogEvents()
    {
        return await _context.LogEvents.ToListAsync();
    }
    */

    /*

    // GET: api/LogEvents/5
    [HttpGet("{id}")]
    public async Task<ActionResult<LogEvent>> GetLogEvent(Guid id)
    {
        var logEvent = await _context.LogEvents.FindAsync(id);

        if (logEvent == null)
        {
            return NotFound();
        }

        return logEvent;
    }

    // PUT: api/LogEvents/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLogEvent(Guid id, LogEvent logEvent)
    {
        if (id != logEvent.Id)
        {
            return BadRequest();
        }

        _context.Entry(logEvent).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LogEventExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }



    // DELETE: api/LogEvents/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLogEvent(Guid id)
    {
        var logEvent = await _context.LogEvents.FindAsync(id);
        if (logEvent == null)
        {
            return NotFound();
        }

        _context.LogEvents.Remove(logEvent);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool LogEventExists(Guid id)
    {
        return _context.LogEvents.Any(e => e.Id == id);
    }
    */
}