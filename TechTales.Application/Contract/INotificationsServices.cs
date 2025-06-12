using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Infrastructure.Models;

namespace TechTales.Application.Contract
{
    public interface INotificationsServices
    {

        Task SendEmail(string to, string subject, string body, bool isHtml = false);

        string LoadEmbeddedTemplate(string templateName);

        string RenderTemplateWelcome(string templateName, UsersModel model);

        UsersModel GenerateUsuarioModel(string nombre, string email, DateTime? fechaRegistro, string password);

    }
}
