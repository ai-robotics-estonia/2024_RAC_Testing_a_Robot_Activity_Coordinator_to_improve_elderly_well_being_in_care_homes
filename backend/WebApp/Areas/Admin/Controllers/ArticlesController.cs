using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using Base.Domain;
using Microsoft.AspNetCore.Authorization;
using WebApp.Areas.Admin.ViewModels;
using LangStr = App.Domain.LangStr;

namespace WebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin")]
public class ArticlesController : Controller
{
    private readonly AppDbContext _context;

    public ArticlesController(AppDbContext context)
    {
        _context = context;
    }

    
    // GET: Admin/Articles
    public async Task<IActionResult> Index()
    {
        var res = await _context.Articles
            .Include(a => a.RobotMapApp)
            .Include(a => a.ArticleCategory)
            .OrderBy(r => r.RobotMapAppId)
            .ThenBy(a => a.ArticleCategoryId)
            .ThenByDescending(a => a.Date)
            .ToListAsync();
        return View(res);
    }

    // GET: Admin/Articles/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var article = await _context.Articles
            .Include(a => a.RobotMapApp)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (article == null)
        {
            return NotFound();
        }

        return View(article);
    }

    // GET: Admin/Articles/Create
    public async Task<IActionResult> Create()
    {
        var vm = new ArticleCreateEditViewModel
        {
            RobotMapAppSelectList = await GetRobotMapAppSelectListAsync(),
            ArticleCategorySelectList = await GetArticleCategorySelectListAsync()
        };


        return View(vm);
    }

    // POST: Admin/Articles/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        ArticleCreateEditViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var article = new Article
            {
                Date = vm.Date,
                DisplayText = new LangStr(vm.DisplayText),
                PlainText = new LangStr(vm.PlainText),
                RobotMapAppId = vm.RobotMapAppId,
                ArticleCategoryId = vm.ArticleCategoryId
            };

            _context.Add(article);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.RobotMapAppSelectList = await GetRobotMapAppSelectListAsync(vm.RobotMapAppId);
        vm.ArticleCategorySelectList = await GetArticleCategorySelectListAsync(vm.ArticleCategoryId);

        return View(vm);
    }

    // GET: Admin/Articles/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var article = await _context.Articles.FindAsync(id);
        if (article == null)
        {
            return NotFound();
        }

        var vm = new ArticleCreateEditViewModel
        {
            Id = article.Id,
            Date = article.Date,
            DisplayText = article.DisplayText,
            PlainText = article.PlainText,
            RobotMapAppId = article.RobotMapAppId,
            RobotMapAppSelectList = await GetRobotMapAppSelectListAsync(article.RobotMapAppId),
            ArticleCategoryId = article.ArticleCategoryId,
            ArticleCategorySelectList = await GetArticleCategorySelectListAsync(article.ArticleCategoryId),
        };

        return View(vm);
    }

    // POST: Admin/Articles/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        ArticleCreateEditViewModel vm)
    {
        if (id != vm.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var dbArticle = await _context.Articles.SingleAsync(a => a.Id == vm.Id);
                dbArticle.Date = vm.Date;
                dbArticle.RobotMapAppId = vm.RobotMapAppId;
                dbArticle.DisplayText.SetTranslation(vm.DisplayText);
                dbArticle.PlainText.SetTranslation(vm.PlainText);
                
                dbArticle.ArticleCategoryId = vm.ArticleCategoryId;
                
                _context.Articles.Update(dbArticle);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(vm.Id))
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

        vm.RobotMapAppSelectList = await GetRobotMapAppSelectListAsync(vm.RobotMapAppId);
        vm.ArticleCategorySelectList = await GetArticleCategorySelectListAsync(vm.ArticleCategoryId);
        return View(vm);
    }

    // GET: Admin/Articles/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var article = await _context.Articles
            .Include(a => a.RobotMapApp)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (article == null)
        {
            return NotFound();
        }

        return View(article);
    }

    // POST: Admin/Articles/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article != null)
        {
            _context.Articles.Remove(article);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ArticleExists(Guid id)
    {
        return _context.Articles.Any(e => e.Id == id);
    }

    private async Task<SelectList> GetArticleCategorySelectListAsync(Guid? selectedValue = null)
    {
        var articleCategories = await _context.ArticleCategories
            .Include(a => a.Organization)
            .OrderBy(o => o.CategoryName)
            .Select(i => new KeyValue()
            {
                Key = i.Id,
                Value =  $"{i.CategoryName} ({i.Organization!.OrgName})"
            })
            .ToListAsync();

        return new SelectList(articleCategories, nameof(KeyValue.Key), nameof(KeyValue.Value),
            selectedValue);
    }

    private async Task<SelectList> GetRobotMapAppSelectListAsync(Guid? selectedValue = null)
    {
        var robotMapApps = await _context.RobotMapApps
            .OrderBy(o => o.DisplayName)
            .ToListAsync();

        return new SelectList(robotMapApps, nameof(RobotMapApp.Id), nameof(RobotMapApp.DisplayName), selectedValue);
    }
}