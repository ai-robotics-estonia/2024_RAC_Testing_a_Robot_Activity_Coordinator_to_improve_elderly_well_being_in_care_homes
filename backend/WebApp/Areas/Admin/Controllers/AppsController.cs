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

namespace WebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin")]
public class AppsController : Controller
{
    private readonly AppDbContext _context;

    public AppsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Admin/Apps
    public async Task<IActionResult> Index()
    {
        return View(await _context.Apps.ToListAsync());
    }

    // GET: Admin/Apps/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var app = await _context.Apps
            .FirstOrDefaultAsync(m => m.Id == id);
        if (app == null)
        {
            return NotFound();
        }

        return View(app);
    }

    // GET: Admin/Apps/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/Apps/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("AppName,Id")] App.Domain.App app)
    {
        if (ModelState.IsValid)
        {
            app.Id = Guid.NewGuid();
            _context.Add(app);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(app);
    }

    // GET: Admin/Apps/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var app = await _context.Apps.FindAsync(id);
        if (app == null)
        {
            return NotFound();
        }
        return View(app);
    }

    // POST: Admin/Apps/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("AppName,Id")] App.Domain.App app)
    {
        if (id != app.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(app);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppExists(app.Id))
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
        return View(app);
    }

    // GET: Admin/Apps/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var app = await _context.Apps
            .FirstOrDefaultAsync(m => m.Id == id);
        if (app == null)
        {
            return NotFound();
        }

        return View(app);
    }

    // POST: Admin/Apps/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var app = await _context.Apps.FindAsync(id);
        if (app != null)
        {
            _context.Apps.Remove(app);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AppExists(Guid id)
    {
        return _context.Apps.Any(e => e.Id == id);
    }
}