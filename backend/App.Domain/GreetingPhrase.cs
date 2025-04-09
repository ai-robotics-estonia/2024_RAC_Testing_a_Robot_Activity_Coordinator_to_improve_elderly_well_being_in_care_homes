using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class GreetingPhrase : BaseEntity
{
    [Column(TypeName = "jsonb")]
    public LangStr Phrase { get; set; } = default!;
    
    public Guid RobotMapAppId { get; set; }
    public RobotMapApp? RobotMapApp { get; set; }

    public Guid GreetingPhraseCategoryId { get; set; }
    public GreetingPhraseCategory? GreetingPhraseCategory { get; set; }
}