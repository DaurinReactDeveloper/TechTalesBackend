using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Domain.entities;

namespace TechTales.Infrastructure.Models
{
    public partial class CommentsModel
    {

        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public DateTime? DateComment { get; set; }

        public int IdUser { get; set; }

        public int? IdStorie { get; set; }

    }
}
