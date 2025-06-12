using System;
using System.Collections.Generic;
using TechTales.Domain.Core;

namespace TechTales.Domain.entities;

public partial class Users : SimilarFields
{

    public int Id { get; set; }

    public string ImgProfile { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Role { get; set; }

    public DateTime? DateRegister { get; set; }

    public virtual ICollection<Comments> Comments { get; set; } = new List<Comments>();

    public virtual ICollection<Stories> Stories { get; set; } = new List<Stories>();

    public virtual ICollection<Challenges> Challenges { get; set; } = new List<Challenges>();

    public virtual ICollection<ChallengesCompleted> ChallengesCompleted { get; set; } = new List<ChallengesCompleted>();

    public virtual ICollection<VotesStories> VotesStories { get; set; } = new List<VotesStories>();


}
