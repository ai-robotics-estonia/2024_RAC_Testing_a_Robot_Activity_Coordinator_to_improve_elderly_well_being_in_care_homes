using System.Xml.Linq;
using App.DAL.EF;
using App.Domain;
using App.Domain.Attendance;
using App.DTO;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace App.BLL;

public class DataService
{
    private readonly AppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<DataService> _logger;

    public DataService(AppDbContext context, ILogger<DataService> logger, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }


    public async Task<LectureAttendance> MapAttendanceAsync(AttendanceDTO attendance)
    {
        var res = new LectureAttendance();
        res.UserName = attendance.UserName;
        res.DT = DateTime.Now.ToUniversalTime();
        res.IsRegistration = attendance.IsRegistration;
        if (attendance.LectureId != null)
        {
            res.LectureId = attendance.LectureId.Value; 
        }
        else
        {
            var lecture = await _context.Lectures.FirstOrDefaultAsync();
            if (lecture == null)
            {
                lecture = new Lecture()
                {
                    Id = Guid.NewGuid(),
                    Name = "Default lecture",
                };
                _context.Lectures.Add(lecture);
            }
            res.LectureId = lecture.Id;
        }

        return res;
    }

    public async Task<LogEvent> MapLogEventAsync(LogEventDTO logEvent)
    {
        var res = new LogEvent();
        res.AppLaunch = logEvent.AppLaunch;
        res.Tag = logEvent.Tag;
        res.Message = logEvent.Message;
        res.IntValue = logEvent.IntValue;
        res.DoubleValue = logEvent.DoubleValue;

        res.RobotMapAppId = await GetOrCreateRobotMapAppIdAsync(logEvent.AndroidIdCode, logEvent.MapIdCode,
            logEvent.MapName, logEvent.AppName);

        return res;
    }

    public async Task<Guid> GetOrCreateRobotMapAppIdAsync(string androidIdCode, string mapIdCode, string mapName,
        string appName)
    {
        // check for elements.
        // TODO: CACHE!!!!
        var robot = await _context.Robots.FirstOrDefaultAsync(r => r.AndroidIdCode == androidIdCode);
        if (robot == null)
        {
            robot = new Robot()
            {
                AndroidIdCode = androidIdCode,
                RobotName = androidIdCode,
            };
            _context.Robots.Add(robot);
        }

        var map = await _context.Maps.FirstOrDefaultAsync(m => m.MapIdCode == mapIdCode);
        if (map == null)
        {
            map = new Map()
            {
                MapIdCode = mapIdCode,
                MapName = mapName,
            };
            _context.Maps.Add(map);
        }

        var app = await _context.Apps.FirstOrDefaultAsync(a => a.AppName == appName);
        if (app == null)
        {
            app = new Domain.App()
            {
                AppName = appName,
            };
            _context.Apps.Add(app);
        }


        var robotMapApp =
            await _context.RobotMapApps.FirstOrDefaultAsync(r =>
                r.RobotId == robot.Id && r.MapId == map.Id && r.AppId == app.Id);
        if (robotMapApp == null)
        {
            robotMapApp = new RobotMapApp()
            {
                Robot = robot,
                Map = map,
                App = app,
                DisplayName = app.AppName + " " + map.MapName + " " + robot.RobotName,
            };
            _context.RobotMapApps.Add(robotMapApp);
            await _context.SaveChangesAsync();
        }

        return robotMapApp.Id;
    }

    public async Task<Result<MapLocationUpdateResult>> UpdateMapLocationAsync(MapLocationsDTO mapLocationsDTO)
    {
        var map = await _context.Maps
            .Include(m => m.MapLocations)
            .FirstOrDefaultAsync(m => m.MapIdCode == mapLocationsDTO.MapIdCode);

        if (map == null)
        {
            return Result.Fail($"Map '{mapLocationsDTO.MapName}' not found");
        }

        var res = new MapLocationUpdateResult();

        foreach (var mapLocation in map.MapLocations!)
        {
            if (mapLocationsDTO.MapLocations.Any(m => m == mapLocation.LocationName)) continue;

            _context.MapLocations.Remove(mapLocation);
            res.Removed++;
        }

        foreach (var mapLocation in mapLocationsDTO.MapLocations)
        {
            if (map.MapLocations!.Any(m => m.LocationName == mapLocation)) continue;

            _context.MapLocations.Add(new MapLocation()
            {
                LocationName = mapLocation,
                MapId = map.Id,
                LocationDisplayName = new LangStr(mapLocation)
            });
            res.Added++;
        }

        await _context.SaveChangesAsync();

        return Result.Ok(res);
    }

    public async Task<Result<int>> FixMissingFloorsAsync()
    {
        /*
        var maps = await _context.Maps
            .Include(m => m.MapFloors!)
            .ThenInclude(f => f.MapLocations)
            .Include(m => m.MapLocations)
            .Where(m => m.MapLocations!.Any(l => l.MapFloorId == null))
            .ToListAsync();
        */

        var locations = await _context.MapLocations
            .Include(l => l.Map)
            .ThenInclude(m => m!.MapFloors)
            .Where(l => l.MapFloorId == null)
            .ToListAsync();

        var res = 0;

        foreach (var location in locations)
        {
            // is there a floor in this map?
            var floor = location.Map!.MapFloors!.FirstOrDefault(f => f.FloorName == "1");
            if (floor == null)
            {
                floor = new MapFloor()
                {
                    Id = Guid.NewGuid(),
                    FloorName = "1",
                    FloorDisplayName = new LangStr("1"),
                    MapId = location.MapId,
                    MapLocations = new List<MapLocation>()
                };
                _context.MapFloors.Add(floor);
                location.Map.MapFloors!.Add(floor);
                
            }
            
            res++;
            location.MapFloorId = floor.Id;
        }

        await _context.SaveChangesAsync();

        return Result.Ok(res);
    }

    public async Task<Result<MapUpdateResult>> UpdateMapAsync(MapDTO mapDTO)
    {
        var dbMap = await _context.Maps
            .Include(m => m.MapLocations)
            .Include(m => m.MapFloors!)
            .ThenInclude(f => f.MapLocations)
            .FirstOrDefaultAsync(m => m.MapIdCode == mapDTO.MapIdCode);

        if (dbMap == null)
        {
            return Result.Fail($"Map name:'{mapDTO.MapName}' id:'{mapDTO.MapIdCode}' - not found");
        }

        var res = new MapUpdateResult();

        // remove db floors/locations not found in DTO from db
        foreach (var dbMapFloor in dbMap.MapFloors!)
        {
            if (mapDTO.MapFloors.Any(f => f.FloorName == dbMapFloor.FloorName)) continue;

            _context.MapFloors.Remove(dbMapFloor);
            res.FloorsRemoved++;
        }

        // add/update floors from DTO
        foreach (var dtoMapFloor in mapDTO.MapFloors)
        {
            // maybe floor exists in db
            var dbFloor = dbMap.MapFloors.FirstOrDefault(f => f.FloorName == dtoMapFloor.FloorName);

            // it is a new floor
            if (dbFloor == null)
            {
                dbFloor = new MapFloor()
                {
                    Id = Guid.NewGuid(),
                    FloorName = dtoMapFloor.FloorName,
                    FloorDisplayName = new LangStr(dtoMapFloor.FloorName),
                    MapId = dbMap.Id,
                    MapLocations = new List<MapLocation>()
                };
                _context.MapFloors.Add(dbFloor);
                res.FloorsAdded++;
                //dbMap.MapFloors.Add(dbFloor);
            }

            // locations remove
            foreach (var dbFloorLocation in dbFloor.MapLocations!)
            {
                if (dtoMapFloor.MapLocations.Any(f => f == dbFloorLocation.LocationName)) continue;

                _context.MapLocations.Remove(dbFloorLocation);
                res.LocationsRemoved++;
            }

            //locations add
            // fix me - update existing locations with floor info
            
            foreach (var dtoMapFloorLocation in dtoMapFloor.MapLocations)
            {
                // maybe location already exists correctly with floor info
                if (dbFloor.MapLocations.Any(l => l.LocationName == dtoMapFloorLocation)) continue;
                
                // maybe location exists for map, but no floor is attached
                if (dbMap.MapLocations!.Any(ml => ml.LocationName == dtoMapFloorLocation && ml.MapFloorId == null))
                {
                    var dbMapLocation = dbMap.MapLocations!.First(ml => ml.LocationName == dtoMapFloorLocation);
                    dbMapLocation.MapFloorId = dbFloor.Id;
                    res.LocationsFixed++;
                    continue;
                }

                // it is a new location
                _context.MapLocations.Add(new MapLocation()
                {
                    LocationName = dtoMapFloorLocation,
                    LocationDisplayName = new LangStr(dtoMapFloorLocation),
                    MapId = dbMap.Id,
                    MapFloorId = dbFloor.Id,
                });
                res.LocationsAdded++;
            }
        }

        await _context.SaveChangesAsync();

        return Result.Ok(res);
    }

    public async Task<Result<List<NewsItemDTO>>> GetErrNewsAsync()
    {
        var httpClient = _httpClientFactory.CreateClient();

        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            "https://www.err.ee/rss")
        {
            Headers =
            {
                { HeaderNames.UserAgent, "Aire Temi" }
            }
        };

        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            return Result.Fail("News fetching error! " + httpResponseMessage.StatusCode);
        }

        var xmlBody = await httpResponseMessage.Content.ReadAsStringAsync();
        var xml = XDocument.Parse(xmlBody);

        var newsItems = xml.Root!.Descendants("channel").Descendants("item").Select(x => new NewsItemDTO()
        {
            Title = x.Descendants("title").First().Value + ".",
            Link = x.Descendants("link").First().Value,
            Description = x.Descendants("description").First().Value,
            PubDate = DateTime.Parse(x.Descendants("pubDate").First().Value),
            Category = x.Descendants("category").First().Value,
            ShortLink = x.Descendants("guid").First().Value,
        }).ToList();

        return Result.Ok(newsItems);
    }
}