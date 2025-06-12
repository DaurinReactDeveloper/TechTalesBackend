using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTales.Application.Validations
{
    public static class CloudinaryValidations
    {

        public static bool IsValidImage(string ImageUrl,out string MessageImage)
        {

            MessageImage = string.Empty;

            if (string.IsNullOrEmpty(ImageUrl))
            {
                MessageImage = "La imagen no puede estar vacía.";
                return false;
            }

            return true;

        }

    }
}
