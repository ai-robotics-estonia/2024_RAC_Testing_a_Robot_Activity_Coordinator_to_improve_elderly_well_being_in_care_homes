using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using WebApp.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using WebApp.Helpers;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class ArticleCategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public ArticleCategoriesController(AppDbContext context)
        {
            _context = context;
        }


        // GET: Admin/ArticleCategories
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.ArticleCategories.Include(a => a.Organization);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/ArticleCategories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleCategory = await _context.ArticleCategories
                .Include(a => a.Organization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articleCategory == null)
            {
                return NotFound();
            }

            return View(articleCategory);
        }


        // GET: ArticleCategories/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new ArticleCategoryViewModel
            {
                OrganizationSelectList = await WebHelper.GetOrganizationSelectListAsync(_context),
            };

            return View(viewModel);
        }

        // POST: ArticleCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleCategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.ArticleCategory.CategoryDisplayName = new LangStr(vm.CategoryDisplayName);
                _context.ArticleCategories.Add(vm.ArticleCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new ArticleCategoryViewModel
            {
                OrganizationSelectList =
                    await WebHelper.GetOrganizationSelectListAsync(_context, vm.ArticleCategory.OrganizationId),
            };

            return View(viewModel);
        }

        // GET: ArticleCategories/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var articleCategory = await _context.ArticleCategories.FindAsync(id);

            if (articleCategory == null)
            {
                return NotFound();
            }

            var viewModel = new ArticleCategoryViewModel
            {
                ArticleCategory = articleCategory,
                CategoryDisplayName = articleCategory.CategoryDisplayName,
                OrganizationSelectList =
                    await WebHelper.GetOrganizationSelectListAsync(_context, articleCategory.OrganizationId),
            };

            return View(viewModel);
        }

        // POST: ArticleCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ArticleCategoryViewModel vm)
        {
            if (id != vm.ArticleCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var articleCategory = await _context.ArticleCategories
                    .FindAsync(id);
                if (articleCategory == null) return NotFound();

                articleCategory.CategoryDisplayName.SetTranslation(vm.CategoryDisplayName);

                articleCategory.OrganizationId = vm.ArticleCategory.OrganizationId;
                articleCategory.CategoryName = vm.ArticleCategory.CategoryName;

                _context.Update(articleCategory);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            vm.OrganizationSelectList =
                await WebHelper.GetOrganizationSelectListAsync(_context, vm.ArticleCategory.OrganizationId);

            return View(vm);
        }

        // GET: Admin/ArticleCategories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleCategory = await _context.ArticleCategories
                .Include(a => a.Organization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articleCategory == null)
            {
                return NotFound();
            }

            return View(articleCategory);
        }

        // POST: Admin/ArticleCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var articleCategory = await _context.ArticleCategories.FindAsync(id);
            if (articleCategory != null)
            {
                _context.ArticleCategories.Remove(articleCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}