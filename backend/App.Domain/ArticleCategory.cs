using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class ArticleCategory : BaseEntity
{
    [MaxLength(64)]
    public string CategoryName { get; set; } = default!;

    [Column(TypeName = "jsonb")]
    public LangStr CategoryDisplayName { get; set; } = default!;

    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }

    public ICollection<Article>? Articles { get; set; }
} 