using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using WebApp.Areas.Admin.ViewModels;
using WebApp.Helpers;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WebLinkCategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public WebLinkCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/WebLinkCategories
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.WebLinkCategories.Include(w => w.Organization);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/WebLinkCategories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var webLinkCategory = await _context.WebLinkCategories
                .Include(w => w.Organization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (webLinkCategory == null)
            {
                return NotFound();
            }

            return View(webLinkCategory);
        }

        // GET: Admin/WebLinkCategories/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new WebLinkCategoryViewModel
            {
                OrganizationSelectList = await WebHelper.GetOrganizationSelectListAsync(_context),
            };

            return View(viewModel);
        }

        // POST: Admin/WebLinkCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( WebLinkCategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.WebLinkCategory.CategoryDisplayName = new LangStr(vm.CategoryDisplayName);
                _context.WebLinkCategories.Add(vm.WebLinkCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new WebLinkCategoryViewModel
            {
                OrganizationSelectList =
                    await WebHelper.GetOrganizationSelectListAsync(_context, vm.WebLinkCategory.OrganizationId),
            };

            return View(viewModel);
        }

        // GET: Admin/WebLinkCategories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var webLinkCategory = await _context.WebLinkCategories.FindAsync(id);

            if (webLinkCategory == null)
            {
                return NotFound();
            }

            var viewModel = new WebLinkCategoryViewModel
            {
                WebLinkCategory = webLinkCategory,
                CategoryDisplayName = webLinkCategory.CategoryDisplayName,
                OrganizationSelectList =
                    await WebHelper.GetOrganizationSelectListAsync(_context, webLinkCategory.OrganizationId),
            };

            return View(viewModel);
        }

        // POST: Admin/WebLinkCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, WebLinkCategoryViewModel vm)
        {
            if (id != vm.WebLinkCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var webLinkCategory = await _context.WebLinkCategories
                    .FindAsync(id);
                if (webLinkCategory == null) return NotFound();

                webLinkCategory.CategoryDisplayName.SetTranslation(vm.CategoryDisplayName);

                webLinkCategory.OrganizationId = vm.WebLinkCategory.OrganizationId;
                webLinkCategory.CategoryName = vm.WebLinkCategory.CategoryName;

                _context.Update(webLinkCategory);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            vm.OrganizationSelectList =
                await WebHelper.GetOrganizationSelectListAsync(_context, vm.WebLinkCategory.OrganizationId);

            return View(vm);
        }

        // GET: Admin/WebLinkCategories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var webLinkCategory = await _context.WebLinkCategories
                .Include(w => w.Organization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (webLinkCategory == null)
            {
                return NotFound();
            }

            return View(webLinkCategory);
        }

        // POST: Admin/WebLinkCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var webLinkCategory = await _context.WebLinkCategories.FindAsync(id);
            if (webLinkCategory != null)
            {
                _context.WebLinkCategories.Remove(webLinkCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WebLinkCategoryExists(Guid id)
        {
            return _context.WebLinkCategories.Any(e => e.Id == id);
        }
    }
}
