using System.Net;
using App.BLL;
using App.DAL.EF;
using App.DTO;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AttendanceController: ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AttendanceController> _logger;
    private readonly DataService _dataService;

    public AttendanceController(AppDbContext context, IConfiguration configuration, ILogger<AttendanceController> logger, DataService dataService)
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
    public async Task<ActionResult> PostAttendance([FromBody] AttendanceDTO attendance, [FromQuery] string apiKey)
    {
        if (apiKey != _configuration.GetValue<string>("ApiKey"))
        {
            _logger.LogWarning("Unauth access");
            return BadRequest("Api key problem");
        }
        

        var attendanceEntity = await _dataService.MapAttendanceAsync(attendance);
       

        _context.LectureAttendances.Add(attendanceEntity);
        await _context.SaveChangesAsync();

        return Created("", new { id = attendanceEntity.Id });
        // CreatedAtAction("GetLogEvent", new { id = logEvent.Id }, logEvent);
    }
    
}