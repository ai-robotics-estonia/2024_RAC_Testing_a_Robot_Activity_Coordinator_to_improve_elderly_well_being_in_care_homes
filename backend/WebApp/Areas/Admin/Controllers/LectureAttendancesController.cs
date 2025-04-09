using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain.Attendance;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using WebApp.Areas.Admin.ViewModels;
using X.Extensions.PagedList.EF;

namespace WebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin")]
public class LectureAttendancesController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<LectureAttendancesController> _logger;


    public LectureAttendancesController(AppDbContext context, ILogger<LectureAttendancesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Admin/LectureAttendances
    public async Task<IActionResult> Index(int? page, LectureAttendancesViewModel vm)
    {
        var createdAtFrom = DateTime.Parse(vm.CreatedAtFrom).ToUniversalTime();
        var createdAtTo= DateTime.Parse(vm.CreatedAtTo).ToUniversalTime();
        vm.CreatedAtFrom = createdAtFrom.ToLocalTime().ToString(CultureInfo.CurrentCulture);
        vm.CreatedAtTo = createdAtTo.ToLocalTime().ToString(CultureInfo.CurrentCulture);
        
        var query = _context.LectureAttendances
            .Include(l => l.Lecture)
            .Where(x => x.DT >= createdAtFrom &&
                        x.DT <= createdAtTo)
            .OrderByDescending(l => l.DT)
            .ThenBy(l => l.LectureId)
            .ThenBy(l => l.UserName)
            .AsQueryable();


        if (vm.LectureId != null)
        {
            query= query.Where(x => x.LectureId == vm.LectureId);
        }

        if (vm.EventType != null)
        {
            query = query.Where(x => x.IsRegistration == vm.EventType);
        }

        if (vm.SubmitAction != null && vm.SubmitAction.Equals("csv", StringComparison.CurrentCultureIgnoreCase))
        {
            var csvData = await query.Select(x => new LectureAttendancesCsv()
            {
                DT = x.DT,
                UserName = x.UserName,
                LectureName = x.Lecture!.Name,
                IsRegistration = x.IsRegistration,
            }).ToListAsync();

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            await csv.WriteRecordsAsync(csvData);
            await writer.FlushAsync();
            stream.Position = 0;

            return new FileStreamResult(stream, "text/csv")
            {
                FileDownloadName = "lectureAttendances.csv"
            };
        }


        var pageNumber = page ?? 1;

        vm.PageSizeSelectList = new SelectList(new List<KeyValue<string, int>>()
        {
            new KeyValue<string, int>() { Key = "10", Value = 10 },
            new KeyValue<string, int>() { Key = "25", Value = 25 },
            new KeyValue<string, int>() { Key = "50", Value = 50 },
            new KeyValue<string, int>() { Key = "100", Value = 100 },
            new KeyValue<string, int>() { Key = "200", Value = 200 },
            new KeyValue<string, int>() { Key = "500", Value = 500 },
            new KeyValue<string, int>() { Key = "1000", Value = 1000 },
            new KeyValue<string, int>() { Key = "5000", Value = 5000 },
            new KeyValue<string, int>() { Key = "999999", Value = 999999 },
        }, nameof(KeyValue<string, int>.Key), nameof(KeyValue<string, int>.Value), vm.PageSize);

        vm.LectureSelectList = new SelectList(
            await _context.Lectures
                .OrderBy(x => x.Name)
                .Select(x => new KeyValue<Guid, string>() { Key = x.Id, Value = x.Name })
                .ToListAsync(),
            nameof(KeyValue<string, int>.Key), nameof(KeyValue<string, int>.Value),
            vm.LectureId
        );

        vm.EventTypeSelectList = new SelectList(new List<KeyValue<bool?, string>>()
            {
                new() { Key = true, Value = "Registration" },
                new() { Key = false, Value = "Attendance" },
            },
            nameof(KeyValue<string, int>.Key), nameof(KeyValue<string, int>.Value),
            vm.EventType
        );

        vm.LectureAttendances = await query
            .ToPagedListAsync(pageNumber, vm.PageSize);

        return View(vm);
    }

    // GET: Admin/LectureAttendances/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lectureAttendance = await _context.LectureAttendances
            .Include(l => l.Lecture)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (lectureAttendance == null)
        {
            return NotFound();
        }

        return View(lectureAttendance);
    }

    // GET: Admin/LectureAttendances/Create
    public IActionResult Create()
    {
        ViewData["LectureId"] = new SelectList(_context.Lectures, "Id", "Name");
        return View();
    }

    // POST: Admin/LectureAttendances/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("DT,UserName,LectureId,Id")] LectureAttendance lectureAttendance)
    {
        if (ModelState.IsValid)
        {
            lectureAttendance.Id = Guid.NewGuid();
            _context.Add(lectureAttendance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["LectureId"] = new SelectList(_context.Lectures, "Id", "Name", lectureAttendance.LectureId);
        return View(lectureAttendance);
    }

    // GET: Admin/LectureAttendances/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lectureAttendance = await _context.LectureAttendances.FindAsync(id);
        if (lectureAttendance == null)
        {
            return NotFound();
        }

        ViewData["LectureId"] = new SelectList(_context.Lectures, "Id", "Name", lectureAttendance.LectureId);
        return View(lectureAttendance);
    }

    // POST: Admin/LectureAttendances/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        [Bind("DT,UserName,LectureId,Id")]
        LectureAttendance lectureAttendance)
    {
        if (id != lectureAttendance.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(lectureAttendance);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LectureAttendanceExists(lectureAttendance.Id))
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

        ViewData["LectureId"] = new SelectList(_context.Lectures, "Id", "Name", lectureAttendance.LectureId);
        return View(lectureAttendance);
    }

    // GET: Admin/LectureAttendances/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lectureAttendance = await _context.LectureAttendances
            .Include(l => l.Lecture)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (lectureAttendance == null)
        {
            return NotFound();
        }

        return View(lectureAttendance);
    }

    // POST: Admin/LectureAttendances/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var lectureAttendance = await _context.LectureAttendances.FindAsync(id);
        if (lectureAttendance != null)
        {
            _context.LectureAttendances.Remove(lectureAttendance);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool LectureAttendanceExists(Guid id)
    {
        return _context.LectureAttendances.Any(e => e.Id == id);
    }
}