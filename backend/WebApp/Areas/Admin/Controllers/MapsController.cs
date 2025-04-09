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
public class MapsController : Controller
{
    private readonly AppDbContext _context;

    public MapsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Admin/Maps
    public async Task<IActionResult> Index()
    {
        var maps = await _context.Maps
            .Select(m => new MapViewModel
            {
                Id = m.Id,
                MapName = m.MapName,
                MapIdCode = m.MapIdCode,
                FloorCount = m.MapFloors!.Count,
                LocationsCount = m.MapLocations!.Count
            })
            .ToListAsync();
        
        return View(maps);
    }

    // GET: Admin/Maps/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var map = await _context.Maps
            .FirstOrDefaultAsync(m => m.Id == id);
        if (map == null)
        {
            return NotFound();
        }

        return View(map);
    }

    // GET: Admin/Maps/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/Maps/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MapName,MapIdCode,Id")] Map map)
    {
        if (ModelState.IsValid)
        {
            map.Id = Guid.NewGuid();
            _context.Add(map);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(map);
    }

    // GET: Admin/Maps/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var map = await _context.Maps.FindAsync(id);
        if (map == null)
        {
            return NotFound();
        }
        return View(map);
    }

    // POST: Admin/Maps/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("MapName,MapIdCode,Id")] Map map)
    {
        if (id != map.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(map);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MapExists(map.Id))
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
        return View(map);
    }

    // GET: Admin/Maps/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var map = await _context.Maps
            .FirstOrDefaultAsync(m => m.Id == id);
        if (map == null)
        {
            return NotFound();
        }

        return View(map);
    }

    // POST: Admin/Maps/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var map = await _context.Maps.FindAsync(id);
        if (map != null)
        {
            _context.Maps.Remove(map);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MapExists(Guid id)
    {
        return _context.Maps.Any(e => e.Id == id);
    }
}