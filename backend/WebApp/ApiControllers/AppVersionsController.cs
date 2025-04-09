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
public class AppVersionsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AppVersionsController> _logger;
    private readonly DataService _dataService;
    private string FilePath;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public AppVersionsController(AppDbContext context, IConfiguration configuration,
        ILogger<AppVersionsController> logger, DataService dataService, IWebHostEnvironment hostingEnvironment)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
        _dataService = dataService;
        _hostingEnvironment = hostingEnvironment;
        FilePath = _hostingEnvironment.WebRootPath + System.IO.Path.DirectorySeparatorChar + "uploads" +
                   System.IO.Path.DirectorySeparatorChar;
    }


    [HttpGet]
    [Consumes("application/json")]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<AppVersionDTO>> GetAppVersion(
        [FromQuery] string appName,
        [FromQuery] string apiKey
    )
    {
        if (apiKey != _configuration.GetValue<string>("ApiKey"))
        {
            _logger.LogWarning("Unauth access");
            return BadRequest("Api key problem");
        }

        var app = await _context.Apps.Where(a => a.AppName.ToUpper() == appName.ToUpper()).FirstOrDefaultAsync();
        if (app == null)
        {
            return NotFound("App not found");
        }

        var appVersion = await _context.AppVersions.Where(a => a.AppId == app.Id)
            .OrderByDescending(a => a.ApkVersionCode)
            .FirstOrDefaultAsync();

        if (appVersion == null)
        {
            return NotFound("No appVersion found for app " + appName);
        }

        // TODO: add dl URL
        
        var appVersionDTO = new AppVersionDTO()
        {
            ApkVersionCode = appVersion.ApkVersionCode,
            ApkVersionName = appVersion.ApkVersionName,
            UploadDT = appVersion.UploadDT,
            FileSize = new System.IO.FileInfo(FilePath + appVersion.Id + ".zip").Length,
            Url = $"{Request.Scheme}://{Request.Host}/uploads/{appVersion.Id}.zip"
        };

        return appVersionDTO;
    }
}