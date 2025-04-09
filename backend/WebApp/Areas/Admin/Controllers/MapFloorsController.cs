using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MapFloorsController : Controller
    {
        private readonly AppDbContext _context;

        public MapFloorsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/MapFloors
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.MapFloors
                .Include(m => m.Map)
                .Include(m => m.MapLocations);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<string> Clean()
        {
            var locations = await _context.MapLocations
                .Where(l => l.MapFloorId != null)
                .ToListAsync();

            foreach (var location in locations)
            {
                _context.MapLocations.Remove(location);
            }

            await _context.SaveChangesAsync();

            return "OK";
        }

        // GET: Admin/MapFloors/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mapFloor = await _context.MapFloors
                .Include(m => m.Map)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mapFloor == null)
            {
                return NotFound();
            }

            return View(mapFloor);
        }

        // GET: Admin/MapFloors/Create
        public IActionResult Create()
        {
            ViewData["MapId"] = new SelectList(_context.Maps, "Id", "MapIdCode");
            return View();
        }

        // POST: Admin/MapFloors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FloorName,FloorDisplayName,MapId,Id")] MapFloor mapFloor)
        {
            if (ModelState.IsValid)
            {
                mapFloor.Id = Guid.NewGuid();
                _context.Add(mapFloor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MapId"] = new SelectList(_context.Maps, "Id", "MapIdCode", mapFloor.MapId);
            return View(mapFloor);
        }

        // GET: Admin/MapFloors/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mapFloor = await _context.MapFloors.FindAsync(id);
            if (mapFloor == null)
            {
                return NotFound();
            }

            ViewData["MapId"] = new SelectList(_context.Maps, "Id", "MapIdCode", mapFloor.MapId);
            return View(mapFloor);
        }

        // POST: Admin/MapFloors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("FloorName,FloorDisplayName,MapId,Id")] MapFloor mapFloor)
        {
            if (id != mapFloor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mapFloor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MapFloorExists(mapFloor.Id))
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

            ViewData["MapId"] = new SelectList(_context.Maps, "Id", "MapIdCode", mapFloor.MapId);
            return View(mapFloor);
        }

        // GET: Admin/MapFloors/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mapFloor = await _context.MapFloors
                .Include(m => m.Map)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mapFloor == null)
            {
                return NotFound();
            }

            return View(mapFloor);
        }

        // POST: Admin/MapFloors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var mapFloor = await _context.MapFloors.FindAsync(id);
            if (mapFloor != null)
            {
                _context.MapFloors.Remove(mapFloor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MapFloorExists(Guid id)
        {
            return _context.MapFloors.Any(e => e.Id == id);
        }
    }
}