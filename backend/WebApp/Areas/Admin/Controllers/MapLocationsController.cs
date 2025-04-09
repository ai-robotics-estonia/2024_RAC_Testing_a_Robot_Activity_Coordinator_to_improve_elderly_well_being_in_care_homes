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
public class MapLocationsController : Controller
{
    private readonly AppDbContext _context;

    public MapLocationsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Admin/MapLocations
    public async Task<IActionResult> Index()
    {
        var res = await _context.MapLocations
            .Include(m => m.Map)
            .Include(m => m.MapFloor)
            .OrderBy(m => m.MapId)
            .ThenBy(m => m.MapFloor!.FloorName)
            .ThenBy(m => m.SortPriority)
            .ThenBy(m => m.PatrolPriority)
            .ThenBy(m => m.LocationName)
            .ToListAsync();
        return View(res);
    }

    // GET: Admin/MapLocations/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var mapLocation = await _context.MapLocations
            .Include(m => m.Map)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (mapLocation == null)
        {
            return NotFound();
        }

        return View(mapLocation);
    }

    /*

    // GET: Admin/MapLocations/Create
    public IActionResult Create()
    {
    ViewData["MapId"] = new SelectList(_context.Maps, "Id", "MapIdCode");
    return View();
    }


    // POST: Admin/MapLocations/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("LocationName,LocationDisplayName,SortPriority,PatrolPriority,MapId,Id")] MapLocation mapLocation)
    {
    if (ModelState.IsValid)
    {
        mapLocation.Id = Guid.NewGuid();
        _context.Add(mapLocation);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    ViewData["MapId"] = new SelectList(_context.Maps, "Id", "MapIdCode", mapLocation.MapId);
    return View(mapLocation);
    }
    */

    // GET: Admin/MapLocations/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var mapLocation = await _context.MapLocations
            .Include(m => m.Map)
            .Include(m => m.MapFloor)
            .ThenInclude(mf => mf!.Map)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (mapLocation == null)
        {
            return NotFound();
        }


        var vm = new MapLocationCreateEditViewModel();
        vm.MapLocation = mapLocation;
        vm.LocationDisplayName = mapLocation.LocationDisplayName;
        vm.MapFloorSelectList = new SelectList(
            await _context.MapFloors.Where(m => m.MapId == mapLocation.MapId).ToListAsync(),
            nameof(MapFloor.Id),
            nameof(MapFloor.FloorName)
        );

        return View(vm);
    }

    // POST: Admin/MapLocations/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, MapLocationCreateEditViewModel vm)
    {
        if (id != vm.MapLocation.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var mapLocation = await _context.MapLocations.FindAsync(vm.MapLocation.Id);
                if (mapLocation == null)
                {
                    return NotFound();
                }

                mapLocation.LocationDisplayName.SetTranslation(vm.LocationDisplayName);
                mapLocation.SortPriority = vm.MapLocation.SortPriority;
                mapLocation.PatrolPriority = vm.MapLocation.PatrolPriority;
                mapLocation.MapFloorId = vm.MapLocation.MapFloorId;

                _context.Update(mapLocation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MapLocationExists(vm.MapLocation.Id))
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

        vm.MapLocation = await _context.MapLocations
            .Include(m => m.Map)
            .FirstAsync(m => m.Id == vm.MapLocation.Id);

        //ViewData["MapId"] = new SelectList(_context.Maps, "Id", "MapIdCode", mapLocation.MapId);
        return View(vm);
    }

    // GET: Admin/MapLocations/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var mapLocation = await _context.MapLocations
            .Include(m => m.Map)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (mapLocation == null)
        {
            return NotFound();
        }

        return View(mapLocation);
    }

    // POST: Admin/MapLocations/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var mapLocation = await _context.MapLocations.FindAsync(id);
        if (mapLocation != null)
        {
            _context.MapLocations.Remove(mapLocation);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MapLocationExists(Guid id)
    {
        return _context.MapLocations.Any(e => e.Id == id);
    }
}