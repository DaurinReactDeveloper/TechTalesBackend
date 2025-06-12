using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTales.Application.Dtos.ComentarioDto
{
    public abstract class CommentsDto : DtoBase
    {

        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public DateTime? DateComment { get; set; }

        public int IdUser { get; set; }

        public int? IdStorie { get; set; }

    }
}
