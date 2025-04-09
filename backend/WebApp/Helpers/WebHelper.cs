using App.DAL.EF;
using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Helpers;

public static class WebHelper
{
    public static async Task<SelectList> GetOrganizationSelectListAsync(AppDbContext context,
        Guid? selectedValue = null)
    {
        var organizations = await context.Organizations
            .OrderBy(o => o.OrgName)
            .ToListAsync();

        return new SelectList(organizations, nameof(Organization.Id), nameof(Organization.OrgName), selectedValue);
    }

    public static string Ellipsis(this string str, int length, string ellipsis = "...")
    {
        if (str.Length <= length) return str;
        var substr = str[..(length - ellipsis.Length)];
        return substr + ellipsis;
    }

    public static async Task<SelectList> GetRobotMapAppSelectListAsync(AppDbContext context, Guid userId,
        Guid? selectedValue = null)
    {
        var data = await context.RobotMapApps.Where(r =>
            r.RobotMapAppOrganizations!.Any(ro =>
                ro.Organization!.OrganizationAppUsers!.Any(o =>
                    o.AppUserId.Equals(userId)))
        ).ToListAsync();

        return new SelectList(data, nameof(RobotMapApp.Id), nameof(RobotMapApp.DisplayName), selectedValue);
    }


    public static async Task<SelectList> GetArticleCategorySelectListAsync(AppDbContext context, Guid userId,
        Guid? selectedValue = null)
    {
        var data = await context.ArticleCategories.Where(r =>
            r.Organization!.OrganizationAppUsers!.Any(o => o.AppUserId.Equals(userId)) 
        ).ToListAsync();
        
        return new SelectList(data, nameof(ArticleCategory.Id), nameof(ArticleCategory.CategoryName), selectedValue);
    }

}