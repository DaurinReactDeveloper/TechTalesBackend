using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Domain.entities;

namespace TechTales.Infrastructure.Models
{
    public partial class VotesStoriesModel
    {

        public int Id { get; set; }

        public int IdUser { get; set; }

        public int IdStories { get; set; }

        public bool Vote { get; set; }

        public DateTime? DateVote { get; set; }

    }
}
