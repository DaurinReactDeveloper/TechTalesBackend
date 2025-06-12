using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Domain.entities;

namespace TechTales.Infrastructure.Exceptions
{
    public class CommentsExceptions : Exception
    {

        public CommentsExceptions(string message) : base(message)
        {

        }


    }
}
