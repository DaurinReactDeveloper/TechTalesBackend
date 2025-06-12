using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTales.Application.Contract
{
    public interface ICloudinaryServices
    {

        Task<string?> UploadImageFromUrlAsync(string imageUrl);

    }
}
