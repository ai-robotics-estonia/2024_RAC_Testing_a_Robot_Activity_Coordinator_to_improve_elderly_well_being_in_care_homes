using System.ComponentModel.DataAnnotations;
using App.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.User.ViewModels;

public class ArticleCreateEditViewModel
{
    public Guid Id { get; set; }
    

    
    [Display(Name = nameof(Date), ResourceType = typeof(App.Resources.Domain.Article))]
    public DateOnly? Date { get; set; }
    
    
    [DataType(DataType.Text)]
    [Display(Name = nameof(PlainText), ResourceType = typeof(App.Resources.Domain.Article))]
    public string PlainText { get; set; } = default!;

    
    [DataType(DataType.Text)]
    [Display(Name = nameof(DisplayText), ResourceType = typeof(App.Resources.Domain.Article))]
    public string DisplayText { get; set; } = default!;
    

    [Display(Name = nameof(RobotMapApp), ResourceType = typeof(App.Resources.Domain.Article))]
    public Guid RobotMapAppId { get; set; }
    

    [Display(Name = nameof(ArticleCategory), ResourceType = typeof(App.Resources.Domain.Article))]
    public Guid ArticleCategoryId { get; set; }

    
    [ValidateNever]
    [Display(Name = nameof(RobotMapApp), ResourceType = typeof(App.Resources.Domain.Article))]
    public SelectList RobotMapAppSelectList { get; set; } = default!;
    
    [ValidateNever]
    [Display(Name = nameof(RobotMapApp), ResourceType = typeof(App.Resources.Domain.Article))]
    public SelectList ArticleCategorySelectList { get; set; } = default!;
}