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

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class GreetingPhrasesController : Controller
    {
        private readonly AppDbContext _context;

        public GreetingPhrasesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/GreetingPhrases
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.GreetingPhrases.Include(g => g.GreetingPhraseCategory).Include(g => g.RobotMapApp);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/GreetingPhrases/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var greetingPhrase = await _context.GreetingPhrases
                .Include(g => g.GreetingPhraseCategory)
                .Include(g => g.RobotMapApp)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (greetingPhrase == null)
            {
                return NotFound();
            }

            return View(greetingPhrase);
        }

        // GET: Admin/GreetingPhrases/Create
        public IActionResult Create()
        {
            ViewData["GreetingPhraseCategoryId"] = new SelectList(_context.GreetingPhraseCategories, "Id", "CategoryName");
            ViewData["RobotMapAppId"] = new SelectList(_context.RobotMapApps, "Id", "DisplayName");
            return View();
        }

        // POST: Admin/GreetingPhrases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Phrase,RobotMapAppId,GreetingPhraseCategoryId,Id")] GreetingPhrase greetingPhrase)
        {
            if (ModelState.IsValid)
            {
                greetingPhrase.Id = Guid.NewGuid();
                _context.Add(greetingPhrase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GreetingPhraseCategoryId"] = new SelectList(_context.GreetingPhraseCategories, "Id", "CategoryName", greetingPhrase.GreetingPhraseCategoryId);
            ViewData["RobotMapAppId"] = new SelectList(_context.RobotMapApps, "Id", "DisplayName", greetingPhrase.RobotMapAppId);
            return View(greetingPhrase);
        }

        // GET: Admin/GreetingPhrases/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var greetingPhrase = await _context.GreetingPhrases.FindAsync(id);
            if (greetingPhrase == null)
            {
                return NotFound();
            }
            ViewData["GreetingPhraseCategoryId"] = new SelectList(_context.GreetingPhraseCategories, "Id", "CategoryName", greetingPhrase.GreetingPhraseCategoryId);
            ViewData["RobotMapAppId"] = new SelectList(_context.RobotMapApps, "Id", "DisplayName", greetingPhrase.RobotMapAppId);
            return View(greetingPhrase);
        }

        // POST: Admin/GreetingPhrases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Phrase,RobotMapAppId,GreetingPhraseCategoryId,Id")] GreetingPhrase greetingPhrase)
        {
            if (id != greetingPhrase.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(greetingPhrase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GreetingPhraseExists(greetingPhrase.Id))
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
            ViewData["GreetingPhraseCategoryId"] = new SelectList(_context.GreetingPhraseCategories, "Id", "CategoryName", greetingPhrase.GreetingPhraseCategoryId);
            ViewData["RobotMapAppId"] = new SelectList(_context.RobotMapApps, "Id", "DisplayName", greetingPhrase.RobotMapAppId);
            return View(greetingPhrase);
        }

        // GET: Admin/GreetingPhrases/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var greetingPhrase = await _context.GreetingPhrases
                .Include(g => g.GreetingPhraseCategory)
                .Include(g => g.RobotMapApp)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (greetingPhrase == null)
            {
                return NotFound();
            }

            return View(greetingPhrase);
        }

        // POST: Admin/GreetingPhrases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var greetingPhrase = await _context.GreetingPhrases.FindAsync(id);
            if (greetingPhrase != null)
            {
                _context.GreetingPhrases.Remove(greetingPhrase);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GreetingPhraseExists(Guid id)
        {
            return _context.GreetingPhrases.Any(e => e.Id == id);
        }
    }
}
