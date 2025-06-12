using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTales.Application.Validations
{
    public static class AuthenticationValidations
    {

        public static bool AuthenticationValidationPassword(string passwordUser, string PasswordModel, out string message)
        {

            message = string.Empty;

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(passwordUser, PasswordModel);

            if (!isPasswordValid)
            {
                message = "Contraseña incorrecta.";
                return false;
            }

            return true;

        }

    }
}
