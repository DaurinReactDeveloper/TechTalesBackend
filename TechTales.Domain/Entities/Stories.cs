using System;
using System.Collections.Generic;
using TechTales.Domain.Core;

namespace TechTales.Domain.entities;

public partial class Stories : SimilarFields
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime? DatePublication { get; set; }

    public int IdUser { get; set; }

    public virtual ICollection<Comments> Comments { get; set; } = new List<Comments>();

    public virtual Users IdUserNavigation { get; set; } = null!;

    public virtual ICollection<VotesStories> VotesStories { get; set; } = new List<VotesStories>();

}
