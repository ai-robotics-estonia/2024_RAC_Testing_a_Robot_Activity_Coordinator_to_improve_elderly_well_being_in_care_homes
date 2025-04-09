using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using App.DTO;
using Microsoft.AspNetCore.Authorization;
using WebApp.Areas.Admin.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class AppVersionsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<AppVersionsController> _logger;
        private string FilePath;

        private static readonly JsonSerializerOptions JsonOptionsMeta = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        public AppVersionsController(AppDbContext context, IWebHostEnvironment hostingEnvironment,
            ILogger<AppVersionsController> logger)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;

            FilePath = _hostingEnvironment.WebRootPath + System.IO.Path.DirectorySeparatorChar + "uploads" +
                       System.IO.Path.DirectorySeparatorChar;
        }

        // GET: Admin/AppVersions
        public async Task<IActionResult> Index()
        {
            var data = await _context.AppVersions
                .Include(a => a.App)
                .ToListAsync();

            var vm = new AppVersionsIndexViewModel()
            {
                VersionDetailInfos = data
                    .Select(a =>
                    {
                        var filename = FilePath + a.Id + ".zip";
                        var fileInfo = new System.IO.FileInfo(filename);
                        return new VersionDetailInfo()
                        {
                            AppVersion = a,
                            DT = fileInfo.Exists ? fileInfo.CreationTime : null,
                            FileSize = fileInfo.Exists ? fileInfo.Length : null,
                        };
                    })
                    .ToList(),
            };

            return View(vm);
        }

        // GET: Admin/AppVersions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appVersion = await _context.AppVersions
                .Include(a => a.App)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appVersion == null)
            {
                return NotFound();
            }

            return View(appVersion);
        }

        // GET: Admin/AppVersions/Create
        public IActionResult Create()
        {
            ViewData["AppId"] = new SelectList(_context.Apps, "Id", "AppName");
            return View();
        }

        // POST: Admin/AppVersions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppVersion appVersion)
        {
            appVersion.UploadDT = DateTime.Now;


            if (Request.Form.Files.Count != 2)
            {
                ModelState.AddModelError("file", "No file uploaded");
            }


            if (ModelState.IsValid)
            {
                var filename = Request.Form.Files[0].FileName;
                var extension = System.IO.Path.GetExtension(filename);
                if (!extension.EndsWith("APK", StringComparison.InvariantCultureIgnoreCase))
                {
                    ModelState.AddModelError("file", "File extension must be APK");
                }
            }

            if (ModelState.IsValid)
            {
                var filename = Request.Form.Files[1].FileName;
                var extension = System.IO.Path.GetExtension(filename);
                if (!extension.EndsWith("JSON", StringComparison.InvariantCultureIgnoreCase))
                {
                    ModelState.AddModelError("meta", "File extension must be JSON");
                }
            }


            if (ModelState.IsValid)
            {
                if (Request.Form.Files[0].Length <= 0 || Request.Form.Files[1].Length <= 0)
                {
                    ModelState.AddModelError("file", "File lenght zero");
                }
            }

            AppVersionMetadata? meta = null;
            
            if (ModelState.IsValid)
            {
                meta = await System.Text.Json.JsonSerializer
                    .DeserializeAsync<AppVersionMetadata>(
                        Request.Form.Files[1].OpenReadStream(),
                        JsonOptionsMeta
                    );

                if (meta == null)
                {
                    ModelState.AddModelError("meta", "Cannot deserialize");
                }
            }

            if (ModelState.IsValid)
            {
                appVersion.Id = Guid.NewGuid();
                appVersion.ApkVersionCode = meta!.Elements[0].VersionCode;
                appVersion.ApkVersionName = meta!.Elements[0].VersionName;

                _context.Add(appVersion);
                await _context.SaveChangesAsync();

                DirectoryInfo di = Directory.CreateDirectory(FilePath);
                _logger.LogInformation("Path: " + di.Name + " created");

                var fileNameWithPath = FilePath + System.IO.Path.DirectorySeparatorChar + appVersion.Id + ".zip";

                await using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    await Request.Form.Files[0].CopyToAsync(stream);
                }


                return RedirectToAction(nameof(Index));
            }

            ViewData["AppId"] = new SelectList(_context.Apps, "Id", "AppName", appVersion.AppId);
            return View(appVersion);
        }

        // GET: Admin/AppVersions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appVersion = await _context.AppVersions.FindAsync(id);
            if (appVersion == null)
            {
                return NotFound();
            }

            ViewData["AppId"] = new SelectList(_context.Apps, "Id", "AppName", appVersion.AppId);
            return View(appVersion);
        }

        // POST: Admin/AppVersions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("ApkVersionCode,ApkVersionName,UploadDT,AppId,Id")]
            AppVersion appVersion)
        {
            if (id != appVersion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appVersion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppVersionExists(appVersion.Id))
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

            ViewData["AppId"] = new SelectList(_context.Apps, "Id", "AppName", appVersion.AppId);
            return View(appVersion);
        }

        // GET: Admin/AppVersions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appVersion = await _context.AppVersions
                .Include(a => a.App)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appVersion == null)
            {
                return NotFound();
            }

            return View(appVersion);
        }

        // POST: Admin/AppVersions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var appVersion = await _context.AppVersions.FindAsync(id);
            if (appVersion != null)
            {
                _context.AppVersions.Remove(appVersion);
                await _context.SaveChangesAsync();

                var filename = FilePath + appVersion.Id + ".apk";

                System.IO.File.Delete(filename);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AppVersionExists(Guid id)
        {
            return _context.AppVersions.Any(e => e.Id == id);
        }
    }
}