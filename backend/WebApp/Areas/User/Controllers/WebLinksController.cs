using App.DAL.EF;
using App.Domain;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Admin.ViewModels;

namespace WebApp.Areas.User.Controllers;

[Area("User")]
[Authorize(Roles = "user")]
public class WebLinksController : Controller
{
    private readonly AppDbContext _context;

    public WebLinksController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.GetUserId();

        var res = await _context.WebLinks
            .Include(w => w.Organization)
            .Include(w => w.WebLinkCategory)
            .OrderBy(w => w.WebLinkName)
            .Where(w => w.Organization!.OrganizationAppUsers!.Any(o => o.AppUserId.Equals(userId)))
            .ToListAsync();

        return View(res);
    }

    // GET: Admin/WebLinks/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userId = User.GetUserId();

        var webLink = await _context.WebLinks
            .Include(w => w.Organization)
            .Include(w => w.WebLinkCategory)
            .FirstOrDefaultAsync(m =>
                m.Id == id && m.Organization!.OrganizationAppUsers!.Any(o => o.AppUserId.Equals(userId)));

        if (webLink == null)
        {
            return NotFound();
        }

        return View(webLink);
    }


    // GET: Admin/WebLinks/Create
    public IActionResult Create()
    {
        var userId = User.GetUserId();

        var vm = new WebLinkViewModel
        {
            WebLink = new WebLink(),
            OrganizationSelectList =
                new SelectList(
                    _context.Organizations.Where(o => o.OrganizationAppUsers!.Any(u => u.AppUserId == userId)), "Id",
                    "OrgName"),

            WebLinkCategorySelectList = new SelectList(_context.WebLinkCategories.Include(w => w.Organization)
                    .Where(c => c.Organization!.OrganizationAppUsers!.Any(u => u.AppUserId == userId))
                    .Select(x => new
                    {
                        x.Id,
                        CategoryName = x.CategoryName + " (" + x.Organization!.OrgName + ")"
                    })
                , "Id", "CategoryName")
        };

        return View(vm);
    }

    // POST: Admin/WebLinks/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        WebLinkViewModel vm)
    {
        if (ModelState.IsValid)
        {
            vm.WebLink.WebLinkDisplayName = new LangStr(vm.WebLinkDisplayName);

            _context.Add(vm.WebLink);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        var userId = User.GetUserId();

        vm.OrganizationSelectList =
            new SelectList(_context.Organizations
                    .Where(o => o.OrganizationAppUsers!.Any(u => u.AppUserId == userId))
                , "Id", "OrgName", vm.WebLink.OrganizationId);

        vm.WebLinkCategorySelectList = new SelectList(_context.WebLinkCategories
                .Include(w => w.Organization)
                .Where(c => c.Organization!.OrganizationAppUsers!.Any(u => u.AppUserId == userId))
                .Select(x => new
                {
                    x.Id,
                    CategoryName = x.CategoryName + " (" + x.Organization!.OrgName + ")"
                })
            , "Id", "CategoryName", vm.WebLink.WebLinkCategoryId);


        return View(vm);
    }

    // GET: Admin/WebLinks/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var webLink = await _context.WebLinks.FindAsync(id);
        if (webLink == null)
        {
            return NotFound();
        }

        var userId = User.GetUserId();

        var vm = new WebLinkViewModel
        {
            WebLink = webLink,
            WebLinkDisplayName = webLink.WebLinkDisplayName,
            OrganizationSelectList =
                new SelectList(_context.Organizations
                        .Where(o => o.OrganizationAppUsers!.Any(u => u.AppUserId == userId))
                    , "Id", "OrgName", webLink.OrganizationId),
            WebLinkCategorySelectList = new SelectList(_context.WebLinkCategories
                    .Where(c => c.Organization!.OrganizationAppUsers!.Any(u => u.AppUserId == userId))
                    .Include(w => w.Organization).Select(x => new
                    {
                        x.Id,
                        CategoryName = x.CategoryName + " (" + x.Organization!.OrgName + ")"
                    })
                , "Id", "CategoryName", webLink.WebLinkCategoryId)
        };


        return View(vm);
    }

    // POST: Admin/WebLinks/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        WebLinkViewModel vm)
    {
        if (id != vm.WebLink.Id)
        {
            return NotFound();
        }

        var userId = User.GetUserId();

        var webLink = await _context.WebLinks.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);
        if (webLink == null) return NotFound();

        if (ModelState.IsValid)
        {
            vm.WebLink.WebLinkDisplayName = webLink.WebLinkDisplayName;
            vm.WebLink.WebLinkDisplayName.SetTranslation(vm.WebLinkDisplayName);

            _context.Update(vm.WebLink);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        vm.OrganizationSelectList =
            new SelectList(_context.Organizations
                    .Where(o => o.OrganizationAppUsers!.Any(u => u.AppUserId == userId))
                , "Id", "OrgName", vm.WebLink.OrganizationId);
        vm.WebLinkCategorySelectList = new SelectList(_context.WebLinkCategories
                .Where(c => c.Organization!.OrganizationAppUsers!.Any(u => u.AppUserId == userId))
                .Include(w => w.Organization).Select(x => new
                {
                    x.Id,
                    CategoryName = x.CategoryName + " (" + x.Organization!.OrgName + ")"
                })
            , "Id", "CategoryName", vm.WebLink.WebLinkCategoryId);

        return View(vm);
    }


    // GET: Admin/WebLinks/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userId = User.GetUserId();
        var webLink = await _context.WebLinks
            .Include(w => w.Organization)
            .Include(w => w.WebLinkCategory)
            .FirstOrDefaultAsync(m =>
                m.Id == id && m.Organization!.OrganizationAppUsers!.Any(o => o.AppUserId.Equals(userId)));
        if (webLink == null)
        {
            return NotFound();
        }

        return View(webLink);
    }

    // POST: Admin/WebLinks/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var webLink = await _context.WebLinks.FindAsync(id);
        if (webLink != null)
        {
            _context.WebLinks.Remove(webLink);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}