using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Domain.entities;

namespace TechTales.Infrastructure.Models
{
    public partial class ChallengesCompletedModel
    {

        public int Id { get; set; }

        public int IdUser { get; set; }

        public int IdChallenges { get; set; }

        public DateTime? DateCompleted { get; set; }

        public virtual Challenges IdChallengesNavigation { get; set; } = null!;

        public virtual Users IdUserNavigation { get; set; } = null!;
    }
}
