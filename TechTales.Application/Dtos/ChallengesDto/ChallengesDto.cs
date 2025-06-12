using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTales.Application.Dtos.RetoDto
{
    public abstract class ChallengesDto : DtoBase
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
