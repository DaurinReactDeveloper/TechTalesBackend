using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTales.Application.Contract
{
    public interface IPasswordHelperServices
    {
        string GenerarContrasenaTemporal(int longitud = 10);
        string HashPassword(string password);

    }
}
