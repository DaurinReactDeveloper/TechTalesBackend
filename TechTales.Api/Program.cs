using Microsoft.EntityFrameworkCore;
using TechTales.Loc.Dependencies;
using TechTales.Persistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TechTales.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Add Dependency Injection
            builder.Services.AddCommentsServices();
            builder.Services.AddStoriesServices();
            builder.Services.AddChallengesServices();
            builder.Services.AddChallengesCompletedServices();
            builder.Services.AddUserServices();
            builder.Services.AddVotesStoriesServices();
            builder.Services.AddNotificationDependencies();
            builder.Services.AddCloudinaryDependencies();
            builder.Services.AddPasswordHelperDependencies();

            //DataBase
            builder.Services.AddDbContext<DbtechtalesContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("DbTechtalesContext"), new MySqlServerVersion(new Version(8, 0, 23))));

            //Configurar CORS
            var frontendUrl = builder.Configuration.GetValue<string>("FrontendUrl");

            if (string.IsNullOrEmpty(frontendUrl))
            {

                throw new ArgumentException("FrontendUrl" + "El valor de FrontendUrl no puede ser nulo o vacío.");

            }

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(frontendUrl)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            //Integretion JWT

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });


            var app = builder.Build();

            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
