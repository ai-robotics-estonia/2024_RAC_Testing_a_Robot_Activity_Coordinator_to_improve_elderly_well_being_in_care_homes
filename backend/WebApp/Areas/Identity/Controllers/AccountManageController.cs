using System.Text;
using System.Text.Encodings.Web;
using App.DAL.EF;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using WebApp.Areas.Identity.ViewModels;

namespace WebApp.Areas.Identity.Controllers;

[Area("Identity")]
[Authorize(Roles = "admin")]
public class AccountManageController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public AccountManageController(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult ResetPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Index));
        }

        var user = await _userManager.FindByEmailAsync(vm.Email);

        if (user == null)
        {
            return RedirectToAction(nameof(Index));
        }

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = Url.Page(
            "/Account/ResetPassword",
            pageHandler: null,
            values: new { area = "Identity", code },
            protocol: Request.Scheme);


        var url = HtmlEncoder.Default.Encode(callbackUrl!);

        vm.Url = url;

        return View(vm);
    }
}