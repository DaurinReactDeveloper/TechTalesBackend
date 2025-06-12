using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTales.Application.Dtos.RetosCompletados
{
    public abstract class ChallengesCompletedDto : DtoBase
    {

        public int Id { get; set; }

        public int IdUser { get; set; }

        public int IdChallenges { get; set; }

        public DateTime? DateCompleted { get; set; }

    }
}
