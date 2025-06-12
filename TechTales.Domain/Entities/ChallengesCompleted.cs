using System;
using System.Collections.Generic;
using TechTales.Domain.Core;

namespace TechTales.Domain.entities;

public partial class ChallengesCompleted : SimilarFields
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public int IdChallenges { get; set; }

    public DateTime? DateCompleted { get; set; }

    public virtual Challenges IdChallengesNavigation { get; set; } = null!;

    public virtual Users IdUserNavigation { get; set; } = null!;
}
