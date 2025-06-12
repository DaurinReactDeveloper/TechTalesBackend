using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Contract;
using TechTales.Application.Services;

namespace TechTales.Loc.Dependencies
{
    public static class CloudinaryDependencies
    {

        public static void AddCloudinaryDependencies(this IServiceCollection services)
        {

            services.AddScoped<ICloudinaryServices, CloudinaryService>();

        }

    }
}
