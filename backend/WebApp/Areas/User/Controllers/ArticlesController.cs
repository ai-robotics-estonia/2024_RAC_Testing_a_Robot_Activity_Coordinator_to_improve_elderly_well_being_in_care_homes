using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebApp.Areas.User.ViewModels;
using WebApp.Helpers;


namespace WebApp.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "user")]
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
                .Where(a =>
                    a.RobotMapApp!.RobotMapAppOrganizations!.Any(r =>
                        r.Organization!.OrganizationAppUsers!.Any(o =>
                            o.AppUserId.Equals(User.GetUserId())))
                )
                .OrderByDescending(a => a.RobotMapApp)
                .ThenByDescending(a => a.ArticleCategory!.CategoryName)
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
                .Include(a => a.ArticleCategory)
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
                RobotMapAppSelectList = await WebHelper.GetRobotMapAppSelectListAsync(_context, User.GetUserId()),
                ArticleCategorySelectList =
                    await WebHelper.GetArticleCategorySelectListAsync(_context, User.GetUserId()),
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

            vm.RobotMapAppSelectList =
                await WebHelper.GetRobotMapAppSelectListAsync(_context, User.GetUserId(), vm.RobotMapAppId);
            vm.ArticleCategorySelectList =
                await WebHelper.GetArticleCategorySelectListAsync(_context, User.GetUserId(), vm.ArticleCategoryId);

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
                ArticleCategoryId = article.ArticleCategoryId,
                RobotMapAppSelectList =
                    await WebHelper.GetRobotMapAppSelectListAsync(_context, User.GetUserId(), article.RobotMapAppId),
                ArticleCategorySelectList =
                    await WebHelper.GetArticleCategorySelectListAsync(_context, User.GetUserId(),
                        article.ArticleCategoryId),
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
                var dbArticle = await _context.Articles.SingleAsync(a => a.Id == vm.Id);

                dbArticle.Date = vm.Date;

                dbArticle.ArticleCategoryId = vm.ArticleCategoryId;
                dbArticle.RobotMapAppId = vm.RobotMapAppId;

                dbArticle.DisplayText.SetTranslation(vm.DisplayText);
                dbArticle.PlainText.SetTranslation(vm.PlainText);

                _context.Articles.Update(dbArticle);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            vm.RobotMapAppSelectList =
                await WebHelper.GetRobotMapAppSelectListAsync(_context, User.GetUserId(), vm.RobotMapAppId);
            vm.ArticleCategorySelectList =
                await WebHelper.GetArticleCategorySelectListAsync(_context, User.GetUserId(), vm.ArticleCategoryId);
            
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
                .Include(a => a.ArticleCategory)
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
    }
}