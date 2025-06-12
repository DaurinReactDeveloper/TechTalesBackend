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
    public static class ChallengesCompletedDependencies
    {

        public static void AddChallengesCompletedServices(this IServiceCollection services)
        {
            services.AddScoped<IChallengesCompleted, ChallengesCompletedRepository>();
            services.AddTransient<IChallengesCompletedServices, ChallengesCompletedServices>();
        }

    }
}
