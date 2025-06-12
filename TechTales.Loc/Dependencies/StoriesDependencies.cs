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
    public static class StoriesDependencies
    {

        public static void AddStoriesServices(this IServiceCollection services)
        {

            services.AddScoped<IStories, StoriesRepository>();
        
            services.AddTransient<IStoriesServices, StoriesServices>();

        }


    }
}
