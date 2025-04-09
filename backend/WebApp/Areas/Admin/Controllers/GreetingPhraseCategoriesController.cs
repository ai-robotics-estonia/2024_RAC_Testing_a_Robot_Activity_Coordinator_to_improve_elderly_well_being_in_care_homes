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
using WebApp.Helpers;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class GreetingPhraseCategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public GreetingPhraseCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/GreetingPhraseCategories
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.GreetingPhraseCategories.Include(g => g.Organization);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/GreetingPhraseCategories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var greetingPhraseCategory = await _context.GreetingPhraseCategories
                .Include(g => g.Organization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (greetingPhraseCategory == null)
            {
                return NotFound();
            }

            return View(greetingPhraseCategory);
        }

        // GET: Admin/GreetingPhraseCategories/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new GreetingPhraseCategoryViewModel
            {
                OrganizationSelectList = await WebHelper.GetOrganizationSelectListAsync(_context),
            };

            return View(viewModel);
        }

        // POST: Admin/GreetingPhraseCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GreetingPhraseCategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.GreetingPhraseCategory.CategoryDisplayName = new LangStr(vm.CategoryDisplayName);
                _context.GreetingPhraseCategories.Add(vm.GreetingPhraseCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new GreetingPhraseCategoryViewModel
            {
                OrganizationSelectList =
                    await WebHelper.GetOrganizationSelectListAsync(_context, vm.GreetingPhraseCategory.OrganizationId),
            };

            return View(viewModel);
        }

        // GET: Admin/GreetingPhraseCategories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var greetingPhraseCategory = await _context.GreetingPhraseCategories.FindAsync(id);

            if (greetingPhraseCategory == null)
            {
                return NotFound();
            }

            var viewModel = new GreetingPhraseCategoryViewModel
            {
                GreetingPhraseCategory = greetingPhraseCategory,
                CategoryDisplayName = greetingPhraseCategory.CategoryDisplayName,
                OrganizationSelectList =
                    await WebHelper.GetOrganizationSelectListAsync(_context, greetingPhraseCategory.OrganizationId),
            };

            return View(viewModel);
        }

        // POST: Admin/GreetingPhraseCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, GreetingPhraseCategoryViewModel vm)
        {
            if (id != vm.GreetingPhraseCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var articleCategory = await _context.ArticleCategories
                    .FindAsync(id);
                if (articleCategory == null) return NotFound();

                articleCategory.CategoryDisplayName.SetTranslation(vm.CategoryDisplayName);

                articleCategory.OrganizationId = vm.GreetingPhraseCategory.OrganizationId;
                articleCategory.CategoryName = vm.GreetingPhraseCategory.CategoryName;

                _context.Update(articleCategory);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            vm.OrganizationSelectList =
                await WebHelper.GetOrganizationSelectListAsync(_context, vm.GreetingPhraseCategory.OrganizationId);

            return View(vm);
        }

        // GET: Admin/GreetingPhraseCategories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var greetingPhraseCategory = await _context.GreetingPhraseCategories
                .Include(g => g.Organization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (greetingPhraseCategory == null)
            {
                return NotFound();
            }

            return View(greetingPhraseCategory);
        }

        // POST: Admin/GreetingPhraseCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var greetingPhraseCategory = await _context.GreetingPhraseCategories.FindAsync(id);
            if (greetingPhraseCategory != null)
            {
                _context.GreetingPhraseCategories.Remove(greetingPhraseCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GreetingPhraseCategoryExists(Guid id)
        {
            return _context.GreetingPhraseCategories.Any(e => e.Id == id);
        }
    }
}
