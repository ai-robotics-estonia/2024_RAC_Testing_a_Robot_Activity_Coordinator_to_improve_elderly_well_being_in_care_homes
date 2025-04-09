using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain.Attendance;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin")]
public class LecturesController : Controller
{
    private readonly AppDbContext _context;

    public LecturesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Admin/Lectures
    public async Task<IActionResult> Index()
    {
        return View(await _context.Lectures.ToListAsync());
    }

    // GET: Admin/Lectures/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lecture = await _context.Lectures
            .FirstOrDefaultAsync(m => m.Id == id);
        if (lecture == null)
        {
            return NotFound();
        }

        return View(lecture);
    }

    // GET: Admin/Lectures/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/Lectures/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Id")] Lecture lecture)
    {
        if (ModelState.IsValid)
        {
            lecture.Id = Guid.NewGuid();
            _context.Add(lecture);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(lecture);
    }

    // GET: Admin/Lectures/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lecture = await _context.Lectures.FindAsync(id);
        if (lecture == null)
        {
            return NotFound();
        }

        return View(lecture);
    }

    // POST: Admin/Lectures/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Name,Id")] Lecture lecture)
    {
        if (id != lecture.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(lecture);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LectureExists(lecture.Id))
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

        return View(lecture);
    }

    // GET: Admin/Lectures/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lecture = await _context.Lectures
            .FirstOrDefaultAsync(m => m.Id == id);
        if (lecture == null)
        {
            return NotFound();
        }

        return View(lecture);
    }

    // POST: Admin/Lectures/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var lecture = await _context.Lectures.FindAsync(id);
        if (lecture != null)
        {
            _context.Lectures.Remove(lecture);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool LectureExists(Guid id)
    {
        return _context.Lectures.Any(e => e.Id == id);
    }
}