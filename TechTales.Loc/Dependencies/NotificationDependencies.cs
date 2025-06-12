using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Contract;
using TechTales.Application.Services;

namespace TechTales.Loc.Dependencies
{
    public static class NotificationDependencies
    {

        public static void AddNotificationDependencies(this IServiceCollection services)
        {
            services.AddTransient<INotificationsServices>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var emailSettings = configuration.GetSection("Email");

                return new NotificationService(
                    host: emailSettings["Host"],
                    port: int.Parse(emailSettings["Port"]),
                    enableSsl: bool.Parse(emailSettings["EnableSsl"]),
                    userName: emailSettings["UserName"],
                    appPassword: emailSettings["AppPassword"],
                    fromAddress: emailSettings["FromAddress"],
                    logger: provider.GetRequiredService<ILogger<NotificationService>>()
                );
            });

        }

    }
}
