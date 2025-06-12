using Microsoft.Extensions.Logging;
using Scriban;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Contract;
using TechTales.Infrastructure.Models;

namespace TechTales.Application.Services
{
    public class NotificationService : INotificationsServices
    {

        private readonly SmtpClient _smtpClient;
        private readonly string _fromAddress;
        private readonly Assembly _assembly;
        private ILogger<NotificationService> _logger;

        public NotificationService(string host, int port, bool enableSsl, string userName, string appPassword, string fromAddress, ILogger<NotificationService> logger)
        {
            _smtpClient = new SmtpClient(host, port)
            {
                EnableSsl = enableSsl,
                Credentials = new NetworkCredential(userName, appPassword)
            };
            _fromAddress = fromAddress;
            _assembly = Assembly.GetExecutingAssembly();
            _logger = logger;
        }

        public UsersModel GenerateUsuarioModel(string name, string email, DateTime? dateRegister, string password)
        {
            return new UsersModel
            {
                Name = name,
                Email = email,
                DateRegister = dateRegister,
                PasswordHash = password
            };
        }

        public string LoadEmbeddedTemplate(string templateName)
        {
            try
            {
                var resourceName = $"TechTales.Application.Templates.{templateName}.html";
                using (var stream = _assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream is null)
                    {
                        throw new FileNotFoundException($"La plantilla {templateName} no fue encontrada como recurso embebido.");
                    }

                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar la plantilla embebida: {ex.Message}");
                return string.Empty;
            }
        }

        public string RenderTemplateWelcome(string templateName, UsersModel model)
        {
            try
            {

                var templateContent = LoadEmbeddedTemplate(templateName);

                if (string.IsNullOrEmpty(templateContent))
                {
                    throw new Exception("El contenido de la plantilla está vacío.");
                }

                var scriptObject = new Scriban.Runtime.ScriptObject();
                scriptObject.Add("Name", model.Name);
                scriptObject.Add("Email", model.Email);
                scriptObject.Add("DateRegister", model.DateRegister);
                scriptObject.Add("Password", model.PasswordHash);

                var template = Template.Parse(templateContent);
                var renderedContent = template.Render(scriptObject);

                return renderedContent;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al renderizar la plantilla: {ex.Message}.");
                return string.Empty;
            }
        }

        public async Task SendEmail(string to, string subject, string body, bool isHtml = false)
        {
            try
            {
                var from = new MailAddress(_fromAddress, "TechTales");
                var toAddress = new MailAddress(to);

                var mailMessage = new MailMessage(from, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml
                };

                await _smtpClient.SendMailAsync(mailMessage);
            }
            catch (SmtpException smtpEx)
            {
                _logger.LogError($"Error de SMTP al enviar el correo a {to}: {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al enviar el correo a {to}: {ex.Message}");
            }
        }

    }
}
