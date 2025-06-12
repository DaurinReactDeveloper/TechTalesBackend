using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Contract;
using TechTales.Application.Services;
using TechTales.Domain.entities;
using TechTales.Persistence.Interfaces;
using TechTales.Persistence.Repositories;

namespace TechTales.Loc.Dependencies
{
    public static class CommentsDependencies
    {

        public static void AddCommentsServices(this IServiceCollection services)
        {

            services.AddScoped<IComments, CommentsRepository>();

            services.AddTransient<ICommentsServices, CommentsServices>();

        }

    }
}
