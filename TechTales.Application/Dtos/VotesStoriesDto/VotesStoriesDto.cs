using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTales.Application.Dtos.VotosHistoriasDto
{
    public class VotesStoriesDto : DtoBase
    {

        public int Id { get; set; }

        public int IdUser { get; set; }

        public int IdStories { get; set; }

        public bool Vote { get; set; }

        public DateTime? DateVote { get; set; }

    }
}
