using System;
using System.Collections.Generic;
using TechTales.Domain.Core;

namespace TechTales.Domain.entities;

public partial class Comments: SimilarFields
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? DateComment { get; set; }

    public int IdUser { get; set; }

    public int? IdStorie { get; set; }

    public virtual Stories? IdStoriesNavigation { get; set; }

    public virtual Users IdUserNavigation { get; set; } = null!;
}
