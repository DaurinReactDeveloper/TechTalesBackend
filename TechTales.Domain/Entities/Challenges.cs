using System;
using System.Collections.Generic;
using TechTales.Domain.Core;

namespace TechTales.Domain.entities;

public partial class Challenges: SimilarFields
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? DatePublication { get; set; }

    public string? Type { get; set; }

    public string Hint { get; set; } = null!;

    public int IdAdmin { get; set; }

    public virtual Users IdAdminNavigation { get; set; } = null!;

    public virtual ICollection<ChallengesCompleted> ChallengesCompleted { get; set; } = new List<ChallengesCompleted>();
}
