using App.DAL.EF;
using App.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Identity.Controllers;

[Area("Identity")]
[Authorize(Roles = "admin")]
public class OrganizationsController : Controller
{
    private readonly AppDbContext _context;

    public OrganizationsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Admin/Organizations
    public async Task<IActionResult> Index()
    {
        return View(await _context.Organizations.ToListAsync());
    }

    // GET: Admin/Organizations/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var organization = await _context.Organizations
            .FirstOrDefaultAsync(m => m.Id == id);
        if (organization == null)
        {
            return NotFound();
        }

        return View(organization);
    }

    // GET: Admin/Organizations/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/Organizations/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("OrgName,Id")] Organization organization)
    {
        if (ModelState.IsValid)
        {
            organization.Id = Guid.NewGuid();
            _context.Add(organization);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(organization);
    }

    // GET: Admin/Organizations/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var organization = await _context.Organizations.FindAsync(id);
        if (organization == null)
        {
            return NotFound();
        }
        return View(organization);
    }

    // POST: Admin/Organizations/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("OrgName,Id")] Organization organization)
    {
        if (id != organization.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(organization);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationExists(organization.Id))
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
        return View(organization);
    }

    // GET: Admin/Organizations/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var organization = await _context.Organizations
            .FirstOrDefaultAsync(m => m.Id == id);
        if (organization == null)
        {
            return NotFound();
        }

        return View(organization);
    }

    // POST: Admin/Organizations/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var organization = await _context.Organizations.FindAsync(id);
        if (organization != null)
        {
            _context.Organizations.Remove(organization);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool OrganizationExists(Guid id)
    {
        return _context.Organizations.Any(e => e.Id == id);
    }
}