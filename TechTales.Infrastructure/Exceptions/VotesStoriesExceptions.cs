using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Domain.entities;

namespace TechTales.Infrastructure.Exceptions
{
    public class VotesStoriesExceptions : Exception
    {

        public VotesStoriesExceptions(string message) : base(message)
        {

        }

    }
}
