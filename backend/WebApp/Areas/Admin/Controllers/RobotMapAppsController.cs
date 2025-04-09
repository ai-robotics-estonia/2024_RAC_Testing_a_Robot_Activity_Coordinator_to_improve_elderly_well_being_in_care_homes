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

namespace WebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin")]
public class RobotMapAppsController : Controller
{
    private readonly AppDbContext _context;

    public RobotMapAppsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Admin/RobotMapApps
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.RobotMapApps
            .Include(r => r.App)
            .Include(r => r.Map)
            .Include(r => r.Robot)
            .Select(r => new RobotMapAppViewModel
            {
                Id = r.Id,
                RobotName = r.Robot!.RobotName,
                MapName = r.Map!.MapName,
                AppName = r.App!.AppName,
                DisplayName = r.DisplayName,
                LogEventsCount = r.LogEvents!.Count,
                ArticlesCount = r.Articles!.Count
            });
        return View(await appDbContext.ToListAsync());
    }

    // GET: Admin/RobotMapApps/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var robotMapApp = await _context.RobotMapApps
            .Include(r => r.App)
            .Include(r => r.Map)
            .Include(r => r.Robot)
            .Select(r => new RobotMapAppViewModel
            {
                Id = r.Id,
                RobotName = r.Robot!.RobotName,
                MapName = r.Map!.MapName,
                AppName = r.App!.AppName,
                DisplayName = r.DisplayName,
                LogEventsCount = r.LogEvents!.Count,
                ArticlesCount = r.Articles!.Count
            })
            .FirstOrDefaultAsync(m => m.Id == id);

        if (robotMapApp == null)
        {
            return NotFound();
        }

        return View(robotMapApp);
    }

    // GET: Admin/RobotMapApps/Create
    public IActionResult Create()
    {
        ViewData["AppId"] = new SelectList(_context.Apps, "Id", "AppName");
        ViewData["MapId"] = new SelectList(_context.Maps, "Id", "MapIdCode");
        ViewData["RobotId"] = new SelectList(_context.Robots, "Id", "AndroidIdCode");
        return View();
    }

    // POST: Admin/RobotMapApps/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("RobotId,MapId,AppId,DisplayName,Id")] RobotMapApp robotMapApp)
    {
        if (ModelState.IsValid)
        {
            robotMapApp.Id = Guid.NewGuid();
            _context.Add(robotMapApp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["AppId"] = new SelectList(_context.Apps, "Id", "AppName", robotMapApp.AppId);
        ViewData["MapId"] = new SelectList(_context.Maps, "Id", "MapIdCode", robotMapApp.MapId);
        ViewData["RobotId"] = new SelectList(_context.Robots, "Id", "AndroidIdCode", robotMapApp.RobotId);
        return View(robotMapApp);
    }

    // GET: Admin/RobotMapApps/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var robotMapApp = await _context.RobotMapApps.FindAsync(id);
        if (robotMapApp == null)
        {
            return NotFound();
        }
        ViewData["AppId"] = new SelectList(_context.Apps, "Id", "AppName", robotMapApp.AppId);
        ViewData["MapId"] = new SelectList(_context.Maps, "Id", "MapIdCode", robotMapApp.MapId);
        ViewData["RobotId"] = new SelectList(_context.Robots, "Id", "AndroidIdCode", robotMapApp.RobotId);
        return View(robotMapApp);
    }

    // POST: Admin/RobotMapApps/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("RobotId,MapId,AppId,DisplayName,Id")] RobotMapApp robotMapApp)
    {
        if (id != robotMapApp.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(robotMapApp);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RobotMapAppExists(robotMapApp.Id))
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
        ViewData["AppId"] = new SelectList(_context.Apps, "Id", "AppName", robotMapApp.AppId);
        ViewData["MapId"] = new SelectList(_context.Maps, "Id", "MapIdCode", robotMapApp.MapId);
        ViewData["RobotId"] = new SelectList(_context.Robots, "Id", "AndroidIdCode", robotMapApp.RobotId);
        return View(robotMapApp);
    }

    // GET: Admin/RobotMapApps/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var robotMapApp = await _context.RobotMapApps
            .Include(r => r.App)
            .Include(r => r.Map)
            .Include(r => r.Robot)
            .Select(r => new RobotMapAppViewModel
            {
                Id = r.Id,
                RobotName = r.Robot!.RobotName,
                MapName = r.Map!.MapName,
                AppName = r.App!.AppName,
                DisplayName = r.DisplayName,
                LogEventsCount = r.LogEvents!.Count,
                ArticlesCount = r.Articles!.Count
            })
            .FirstOrDefaultAsync(m => m.Id == id);

        if (robotMapApp == null)
        {
            return NotFound();
        }

        return View(robotMapApp);
    }

    // POST: Admin/RobotMapApps/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var robotMapApp = await _context.RobotMapApps.FindAsync(id);
        if (robotMapApp != null)
        {
            _context.RobotMapApps.Remove(robotMapApp);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool RobotMapAppExists(Guid id)
    {
        return _context.RobotMapApps.Any(e => e.Id == id);
    }
}