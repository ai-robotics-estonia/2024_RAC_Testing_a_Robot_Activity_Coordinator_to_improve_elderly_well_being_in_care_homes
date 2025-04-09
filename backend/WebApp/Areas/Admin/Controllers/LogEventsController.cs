using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using Microsoft.AspNetCore.Authorization;
using WebApp.Areas.Admin.ViewModels;
using X.Extensions.PagedList.EF;

namespace WebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin")]
public class LogEventsController : Controller
{
    private readonly AppDbContext _context;

    public LogEventsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Admin/LogEvents
    public async Task<IActionResult> Index(int? page, LogEventsViewModel vm)
    {
        var pageNumber = page ?? 1;

        vm.RobotSelectList = new SelectList(_context.Robots, nameof(Robot.Id), nameof(Robot.RobotName), vm.RobotId);
        vm.AppSelectList = new SelectList(_context.Apps, nameof(App.Domain.App.Id), nameof(App.Domain.App.AppName),
            vm.AppId);
        vm.MapSelectList = new SelectList(_context.Maps, nameof(Map.Id), nameof(Map.MapName), vm.MapId);
        vm.PageSizeSelectList = new SelectList(new List<KeyValue<string, int>>()
        {
            new KeyValue<string, int>() { Key = "50", Value = 50 },
            new KeyValue<string, int>() { Key = "100", Value = 100 },
            new KeyValue<string, int>() { Key = "200", Value = 200 },
            new KeyValue<string, int>() { Key = "500", Value = 500 },
            new KeyValue<string, int>() { Key = "1000", Value = 1000 },
            new KeyValue<string, int>() { Key = "5000", Value = 5000 },
            new KeyValue<string, int>() { Key = "999999", Value = 999999 },
        }, nameof(KeyValue<string, int>.Key), nameof(KeyValue<string, int>.Value), vm.PageSize);

        var query =
            _context.LogEvents
                .Include(l => l.RobotMapApp)
                .Where(x => x.CreatedAt >= vm.CreatedAtFrom.ToUniversalTime() &&
                            x.CreatedAt <= vm.CreatedAtTo.ToUniversalTime())
                .OrderByDescending(x => x.CreatedAt)
                .AsQueryable();

        if (!string.IsNullOrEmpty(vm.Tag))
        {
            query = query.Where(x => x.Tag.Contains(vm.Tag));
        }

        if (!string.IsNullOrEmpty(vm.Message))
        {
            query = query.Where(x => x.Message!.Contains(vm.Message));
        }

        if (vm.RobotId != null)
        {
            query = query.Where(l => l.RobotMapApp!.RobotId == vm.RobotId);
        }

        if (vm.MapId != null)
        {
            query = query.Where(l => l.RobotMapApp!.MapId == vm.MapId);
        }

        if (vm.AppId != null)
        {
            query = query.Where(l => l.RobotMapApp!.AppId == vm.AppId);
        }

        vm.LogEvents = await query.ToPagedListAsync(pageNumber, vm.PageSize);

        return View(vm);
    }

    public async Task<IActionResult> Stats(LogEventStatsViewModel vm)
    {
        vm.RobotSelectList = new SelectList(_context.Robots, nameof(Robot.Id), nameof(Robot.RobotName), vm.RobotId);
        vm.AppSelectList = new SelectList(_context.Apps, nameof(App.Domain.App.Id), nameof(App.Domain.App.AppName),
            vm.AppId);
        vm.MapSelectList = new SelectList(_context.Maps, nameof(Map.Id), nameof(Map.MapName), vm.MapId);

        var query =
            _context.LogEvents
                .Include(l => l.RobotMapApp)
                .Where(x => x.CreatedAt >= vm.CreatedAtFrom.ToUniversalTime() &&
                            x.CreatedAt <= vm.CreatedAtTo.ToUniversalTime())
                .AsQueryable();

        if (vm.RobotId != null)
        {
            query = query.Where(l => l.RobotMapApp!.RobotId == vm.RobotId);
        }

        if (vm.MapId != null)
        {
            query = query.Where(l => l.RobotMapApp!.MapId == vm.MapId);
        }

        if (vm.AppId != null)
        {
            query = query.Where(l => l.RobotMapApp!.AppId == vm.AppId);
        }

        vm.Stats = await query
            // x.Tag + " - " + x.RobotMapApp!.DisplayName
            .GroupBy(x => x.Tag)
            .Select(x => new StatItem() { Key = x.Key, Count = x.Count() })
            .OrderBy(x => x.Key)
            .ToListAsync();


        return View(vm);
    }

    public async Task<IActionResult> DetailStats(string key, LogEventStatsViewModel vm)
    {
        var query =
            _context.LogEvents
                .Include(l => l.RobotMapApp)
                .Where(x =>
                    x.CreatedAt >= vm.CreatedAtFrom.ToUniversalTime() &&
                    x.CreatedAt <= vm.CreatedAtTo.ToUniversalTime() &&
                    x.Tag == key
                )
                .AsQueryable();

        if (vm.RobotId != null)
        {
            query = query.Where(l => l.RobotMapApp!.RobotId == vm.RobotId);
        }

        if (vm.MapId != null)
        {
            query = query.Where(l => l.RobotMapApp!.MapId == vm.MapId);
        }

        if (vm.AppId != null)
        {
            query = query.Where(l => l.RobotMapApp!.AppId == vm.AppId);
        }

        var res = new LogEventDetailStatsViewModel();
        res.CreatedAtFrom = vm.CreatedAtFrom;
        res.CreatedAtTo = vm.CreatedAtTo;
        res.Key = key;
        res.RobotName = vm.RobotId == null ? "<any>" : (await _context.Robots.FirstAsync(r => r.Id == vm.RobotId)).RobotName;
        res.MapName = vm.MapId == null ? "<any>" : (await _context.Maps.FirstAsync(m => m.Id == vm.MapId)).MapName;
        res.AppName = vm.AppId == null ? "<any>" : (await _context.Apps.FirstAsync(a => a.Id == vm.AppId)).AppName;

        res.Stats = await query
            .GroupBy(x => x.Message)
            .Select(x => new StatItem() { Key = x.Key!, Count = x.Count() })
            .OrderBy(x => x.Key)
            .ToListAsync();

        return View(res);
    }

    // GET: Admin/LogEvents/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var logEvent = await _context.LogEvents
            .Include(l => l.RobotMapApp)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (logEvent == null)
        {
            return NotFound();
        }

        return View(logEvent);
    }

    // GET: Admin/LogEvents/Create
    public IActionResult Create()
    {
        ViewData["RobotMapAppId"] = new SelectList(_context.RobotMapApps, "Id", "DisplayName");
        return View();
    }

    // POST: Admin/LogEvents/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("CreatedAt,AppLaunch,Tag,Message,IntValue,DoubleValue,RobotMapAppId,Id")]
        LogEvent logEvent)
    {
        if (ModelState.IsValid)
        {
            logEvent.Id = Guid.NewGuid();
            _context.Add(logEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["RobotMapAppId"] = new SelectList(_context.RobotMapApps, "Id", "DisplayName", logEvent.RobotMapAppId);
        return View(logEvent);
    }

    // GET: Admin/LogEvents/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var logEvent = await _context.LogEvents.FindAsync(id);
        if (logEvent == null)
        {
            return NotFound();
        }

        ViewData["RobotMapAppId"] = new SelectList(_context.RobotMapApps, "Id", "DisplayName", logEvent.RobotMapAppId);
        return View(logEvent);
    }

    // POST: Admin/LogEvents/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        [Bind("CreatedAt,AppLaunch,Tag,Message,IntValue,DoubleValue,RobotMapAppId,Id")]
        LogEvent logEvent)
    {
        if (id != logEvent.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(logEvent);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogEventExists(logEvent.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["RobotMapAppId"] = new SelectList(_context.RobotMapApps, "Id", "DisplayName", logEvent.RobotMapAppId);
        return View(logEvent);
    }

    // GET: Admin/LogEvents/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var logEvent = await _context.LogEvents
            .Include(l => l.RobotMapApp)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (logEvent == null)
        {
            return NotFound();
        }

        return View(logEvent);
    }

    // POST: Admin/LogEvents/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var logEvent = await _context.LogEvents.FindAsync(id);
        if (logEvent != null)
        {
            _context.LogEvents.Remove(logEvent);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool LogEventExists(Guid id)
    {
        return _context.LogEvents.Any(e => e.Id == id);
    }
}