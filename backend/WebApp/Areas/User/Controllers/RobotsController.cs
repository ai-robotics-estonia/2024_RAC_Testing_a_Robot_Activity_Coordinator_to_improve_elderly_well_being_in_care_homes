using App.DAL.EF;
using Base.Helpers;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.User.Controllers;

[Area("User")]
[Authorize(Roles = "user")]
public class RobotsController : Controller
{
    private readonly AppDbContext _context;

    public RobotsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var res = await _context.Robots
            .Include(r => r.RobotMapApps!)
            .ThenInclude(x => x.RobotMapAppOrganizations)
            .Where(r => r.RobotMapApps!.Any(rma => rma.RobotMapAppOrganizations!.Any(r =>
                        r.Organization!.OrganizationAppUsers!.Any(o =>
                            o.AppUserId.Equals(User.GetUserId()
                            )
                        )
                    )
                )
            )
            .ToListAsync();
        
        
        return View(res);
    }
}