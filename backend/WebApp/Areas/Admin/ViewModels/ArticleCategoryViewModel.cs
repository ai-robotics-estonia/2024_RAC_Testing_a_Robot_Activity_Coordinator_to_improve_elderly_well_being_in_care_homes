using System.ComponentModel.DataAnnotations;
using App.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels
{
    public class ArticleCategoryViewModel
    {
        [MaxLength(64)]
        public string CategoryDisplayName { get; set; } = default!;
        
        public ArticleCategory ArticleCategory { get; set; } = default!;

        [ValidateNever]
        public SelectList OrganizationSelectList { get; set; } = default!;
    }
} 