using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Domain.entities;

namespace TechTales.Infrastructure.Models
{
    public partial class ChallengesModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateTime? DatePublication { get; set; }

        public string? Type { get; set; }

        public string Hint { get; set; } = null!;

        public int IdAdmin { get; set; }

    }
}
