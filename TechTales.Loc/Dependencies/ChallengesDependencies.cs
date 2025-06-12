using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Contract;
using TechTales.Application.Services;
using TechTales.Persistence.Interfaces;
using TechTales.Persistence.Repositories;

namespace TechTales.Loc.Dependencies
{
    public static class ChallengesDependencies
    {

        public static void AddChallengesServices(this IServiceCollection services)
        {

            services.AddScoped<IChallenges, ChallengesRepository>();

            services.AddTransient<IChallengesServices, ChallengesServices>();

        }

    }
}
