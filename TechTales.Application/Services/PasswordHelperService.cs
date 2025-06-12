using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Contract;

namespace TechTales.Application.Services
{
    public class PasswordHelperService : IPasswordHelperServices
    {
        public string GenerarContrasenaTemporal(int length = 10)
        {
            const string caracteresPermitidos = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%&*";
            var random = new Random();
            return new string(Enumerable.Repeat(caracteresPermitidos, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

    }
}
