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
public class RobotsController : Controller
{
    private readonly AppDbContext _context;

    public RobotsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Admin/Robots
    public async Task<IActionResult> Index()
    {
        return View(await _context.Robots.ToListAsync());
    }

    // GET: Admin/Robots/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var robot = await _context.Robots
            .FirstOrDefaultAsync(m => m.Id == id);
        if (robot == null)
        {
            return NotFound();
        }

        return View(robot);
    }

    // GET: Admin/Robots/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/Robots/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("RobotName,AndroidIdCode,Id")] Robot robot)
    {
        if (ModelState.IsValid)
        {
            robot.Id = Guid.NewGuid();
            _context.Add(robot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(robot);
    }

    // GET: Admin/Robots/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var robot = await _context.Robots.FindAsync(id);
        if (robot == null)
        {
            return NotFound();
        }
        return View(robot);
    }

    // POST: Admin/Robots/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("RobotName,AndroidIdCode,Id")] Robot robot)
    {
        if (id != robot.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(robot);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RobotExists(robot.Id))
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
        return View(robot);
    }

    // GET: Admin/Robots/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var robot = await _context.Robots
            .FirstOrDefaultAsync(m => m.Id == id);
        if (robot == null)
        {
            return NotFound();
        }

        return View(robot);
    }

    // POST: Admin/Robots/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var robot = await _context.Robots.FindAsync(id);
        if (robot != null)
        {
            _context.Robots.Remove(robot);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool RobotExists(Guid id)
    {
        return _context.Robots.Any(e => e.Id == id);
    }
}