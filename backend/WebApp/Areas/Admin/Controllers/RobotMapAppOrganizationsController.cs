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
public class RobotMapAppOrganizationsController : Controller
{
    private readonly AppDbContext _context;

    public RobotMapAppOrganizationsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Admin/RobotMapAppOrganizations
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.RobotMapAppOrganizations.Include(r => r.Organization).Include(r => r.RobotMapApp);
        return View(await appDbContext.ToListAsync());
    }

    // GET: Admin/RobotMapAppOrganizations/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var robotMapAppOrganization = await _context.RobotMapAppOrganizations
            .Include(r => r.Organization)
            .Include(r => r.RobotMapApp)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (robotMapAppOrganization == null)
        {
            return NotFound();
        }

        return View(robotMapAppOrganization);
    }

    // GET: Admin/RobotMapAppOrganizations/Create
    public IActionResult Create()
    {
        ViewData["OrganizationId"] = new SelectList(_context.Organizations, "Id", "OrgName");
        ViewData["RobotMapAppId"] = new SelectList(_context.RobotMapApps, "Id", "DisplayName");
        return View();
    }

    // POST: Admin/RobotMapAppOrganizations/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("RobotMapAppId,OrganizationId,Id")] RobotMapAppOrganization robotMapAppOrganization)
    {
        if (ModelState.IsValid)
        {
            robotMapAppOrganization.Id = Guid.NewGuid();
            _context.Add(robotMapAppOrganization);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["OrganizationId"] = new SelectList(_context.Organizations, "Id", "OrgName", robotMapAppOrganization.OrganizationId);
        ViewData["RobotMapAppId"] = new SelectList(_context.RobotMapApps, "Id", "DisplayName", robotMapAppOrganization.RobotMapAppId);
        return View(robotMapAppOrganization);
    }

    // GET: Admin/RobotMapAppOrganizations/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var robotMapAppOrganization = await _context.RobotMapAppOrganizations.FindAsync(id);
        if (robotMapAppOrganization == null)
        {
            return NotFound();
        }
        ViewData["OrganizationId"] = new SelectList(_context.Organizations, "Id", "OrgName", robotMapAppOrganization.OrganizationId);
        ViewData["RobotMapAppId"] = new SelectList(_context.RobotMapApps, "Id", "DisplayName", robotMapAppOrganization.RobotMapAppId);
        return View(robotMapAppOrganization);
    }

    // POST: Admin/RobotMapAppOrganizations/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("RobotMapAppId,OrganizationId,Id")] RobotMapAppOrganization robotMapAppOrganization)
    {
        if (id != robotMapAppOrganization.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(robotMapAppOrganization);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RobotMapAppOrganizationExists(robotMapAppOrganization.Id))
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
        ViewData["OrganizationId"] = new SelectList(_context.Organizations, "Id", "OrgName", robotMapAppOrganization.OrganizationId);
        ViewData["RobotMapAppId"] = new SelectList(_context.RobotMapApps, "Id", "DisplayName", robotMapAppOrganization.RobotMapAppId);
        return View(robotMapAppOrganization);
    }

    // GET: Admin/RobotMapAppOrganizations/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var robotMapAppOrganization = await _context.RobotMapAppOrganizations
            .Include(r => r.Organization)
            .Include(r => r.RobotMapApp)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (robotMapAppOrganization == null)
        {
            return NotFound();
        }

        return View(robotMapAppOrganization);
    }

    // POST: Admin/RobotMapAppOrganizations/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var robotMapAppOrganization = await _context.RobotMapAppOrganizations.FindAsync(id);
        if (robotMapAppOrganization != null)
        {
            _context.RobotMapAppOrganizations.Remove(robotMapAppOrganization);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool RobotMapAppOrganizationExists(Guid id)
    {
        return _context.RobotMapAppOrganizations.Any(e => e.Id == id);
    }
}