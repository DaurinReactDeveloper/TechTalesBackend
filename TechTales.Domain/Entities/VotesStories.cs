using System;
using System.Collections.Generic;
using TechTales.Domain.Core;

namespace TechTales.Domain.entities;

public partial class VotesStories : SimilarFields
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public int IdStories { get; set; }

    public bool Vote { get; set; }

    public DateTime? DateVote { get; set; }

    public virtual Stories IdStoriesNavigation { get; set; } = null!;

    public virtual Users IdUserNavigation { get; set; } = null!;
}
