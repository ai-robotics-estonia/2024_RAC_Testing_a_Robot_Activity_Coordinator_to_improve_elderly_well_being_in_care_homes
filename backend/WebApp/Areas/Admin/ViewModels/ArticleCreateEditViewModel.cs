using System.ComponentModel.DataAnnotations;
using App.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels;

public class ArticleCreateEditViewModel
{
    public Guid Id { get; set; }
    
    public DateOnly? Date { get; set; }


    [DataType(DataType.Text)]
    public string PlainText { get; set; } = default!;

    [DataType(DataType.Text)]
    public string DisplayText { get; set; } = default!;

    public Guid RobotMapAppId { get; set; }
    
    [ValidateNever]
    public SelectList RobotMapAppSelectList { get; set; } = default!;


    public Guid ArticleCategoryId { get; set; }
    
    [ValidateNever]
    public SelectList ArticleCategorySelectList { get; set; } = default!;

}