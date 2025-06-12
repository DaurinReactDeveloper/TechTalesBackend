using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Domain.entities;

namespace TechTales.Infrastructure.Exceptions
{
    public class ChallengesExceptions : Exception
    {

        public ChallengesExceptions(string message) : base(message)
        {

        }

    }
}
