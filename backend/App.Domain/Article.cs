using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;

[Index(nameof(ArticleCategoryId), nameof(Date), nameof(RobotMapAppId), IsUnique = true)]
public class Article : BaseEntity
{
    /*
    [MaxLength(128)]
    [Display(Name = nameof(Title), ResourceType = typeof(Resources.Domain.Article))]
    public string Title { get; set; } = default!;
    
    [Column(TypeName = "jsonb")]
    [Display(Name = nameof(DisplayTitle), ResourceType = typeof(Resources.Domain.Article))]
    public LangStr DisplayTitle{ get; set; }  = default!;
    */

    [Display(Name = nameof(Date), ResourceType = typeof(Resources.Domain.Article))]
    public DateOnly? Date { get; set; }

    [Column(TypeName = "jsonb")]
    [Display(Name = nameof(PlainText), ResourceType = typeof(Resources.Domain.Article))]
    public LangStr PlainText { get; set; }  = default!;

    [Column(TypeName = "jsonb")]
    [Display(Name = nameof(DisplayText), ResourceType = typeof(Resources.Domain.Article))]
    public LangStr DisplayText{ get; set; }  = default!;


    [Display(Name = nameof(RobotMapApp), ResourceType = typeof(Resources.Domain.Article))]
    public Guid RobotMapAppId { get; set; }
    [Display(Name = nameof(RobotMapApp), ResourceType = typeof(Resources.Domain.Article))]
    public RobotMapApp? RobotMapApp { get; set; }

    public Guid ArticleCategoryId { get; set; }
    public ArticleCategory? ArticleCategory { get; set; }
}