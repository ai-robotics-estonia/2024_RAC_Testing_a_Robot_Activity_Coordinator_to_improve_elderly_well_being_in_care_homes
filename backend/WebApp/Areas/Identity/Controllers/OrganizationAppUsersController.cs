using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Identity.Controllers;

[Area("Identity")]
[Authorize(Roles = "admin")]
public class OrganizationAppUsersController : Controller
{
    private readonly AppDbContext _context;

    public OrganizationAppUsersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Admin/OrganizationAppUsers
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.OrganizationAppUsers
            .Include(o => o.AppUser)
            .Include(o => o.Organization)
            .OrderBy(c => c.Organization!.OrgName)
            .ThenBy(c => c.AppUser!.Email);

        return View(await appDbContext.ToListAsync());
    }

    // GET: Admin/OrganizationAppUsers/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var organizationAppUser = await _context.OrganizationAppUsers
            .Include(o => o.AppUser)
            .Include(o => o.Organization)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (organizationAppUser == null)
        {
            return NotFound();
        }

        return View(organizationAppUser);
    }

    // GET: Admin/OrganizationAppUsers/Create
    public IActionResult Create()
    {
        ViewData["AppUserId"] = new SelectList(_context.Users, "Id", nameof(AppUser.Email));
        ViewData["OrganizationId"] = new SelectList(_context.Organizations, "Id", "OrgName");
        return View();
    }

    // POST: Admin/OrganizationAppUsers/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OrganizationAppUser organizationAppUser)
    {
        if (ModelState.IsValid)
        {
            organizationAppUser.Id = Guid.NewGuid();
            _context.Add(organizationAppUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["AppUserId"] =
            new SelectList(_context.Users, "Id", nameof(AppUser.Email), organizationAppUser.AppUserId);
        ViewData["OrganizationId"] =
            new SelectList(_context.Organizations, "Id", "OrgName", organizationAppUser.OrganizationId);
        return View(organizationAppUser);
    }

    // GET: Admin/OrganizationAppUsers/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var organizationAppUser = await _context.OrganizationAppUsers.FindAsync(id);
        if (organizationAppUser == null)
        {
            return NotFound();
        }

        ViewData["AppUserId"] =
            new SelectList(_context.Users, "Id", nameof(AppUser.Email), organizationAppUser.AppUserId);
        ViewData["OrganizationId"] =
            new SelectList(_context.Organizations, "Id", "OrgName", organizationAppUser.OrganizationId);
        return View(organizationAppUser);
    }

    // POST: Admin/OrganizationAppUsers/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, OrganizationAppUser organizationAppUser)
    {
        if (id != organizationAppUser.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(organizationAppUser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationAppUserExists(organizationAppUser.Id))
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

        ViewData["AppUserId"] =
            new SelectList(_context.Users, "Id", nameof(AppUser.Email), organizationAppUser.AppUserId);
        ViewData["OrganizationId"] =
            new SelectList(_context.Organizations, "Id", "OrgName", organizationAppUser.OrganizationId);
        return View(organizationAppUser);
    }

    // GET: Admin/OrganizationAppUsers/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var organizationAppUser = await _context.OrganizationAppUsers
            .Include(o => o.AppUser)
            .Include(o => o.Organization)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (organizationAppUser == null)
        {
            return NotFound();
        }

        return View(organizationAppUser);
    }

    // POST: Admin/OrganizationAppUsers/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var organizationAppUser = await _context.OrganizationAppUsers.FindAsync(id);
        if (organizationAppUser != null)
        {
            _context.OrganizationAppUsers.Remove(organizationAppUser);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool OrganizationAppUserExists(Guid id)
    {
        return _context.OrganizationAppUsers.Any(e => e.Id == id);
    }
}