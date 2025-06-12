using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTales.Infrastructure.Exceptions
{
    public class UsersExcepetions : Exception
    {
        public UsersExcepetions(string message) : base(message)
        {

        }
    }
}
