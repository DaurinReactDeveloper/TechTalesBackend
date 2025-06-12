using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTales.Infrastructure.Exceptions
{
    public class StoriesExceptions : Exception
    {
        public StoriesExceptions(string message) : base(message)
        {

        }
    }
}
