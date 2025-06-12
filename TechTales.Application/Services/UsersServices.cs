using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Contract;
using TechTales.Application.Core;
using TechTales.Application.Dtos.UsuarioDto;
using TechTales.Application.Validations;
using TechTales.Application.Validations;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Models;
using TechTales.Persistence.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechTales.Application.Services
{
    public class UsersServices : IUsersServices
    {
        private readonly IUsers _user;
        private readonly ILogger<UsersServices> _logger;
        private readonly INotificationsServices _emailServices;
        private readonly ICloudinaryServices _cloudinaryServices;
        private readonly IPasswordHelperServices _passwordHelperServices;

        public UsersServices(IUsers user, ILogger<UsersServices> logger, INotificationsServices emailServices, ICloudinaryServices cloudinaryServices, IPasswordHelperServices passwordHelperServices)
        {
            this._user = user;
            this._logger = logger;
            this._emailServices = emailServices;
            this._cloudinaryServices = cloudinaryServices;
            this._passwordHelperServices = passwordHelperServices;
        }

        public async Task<ServiceResult> GetUsers()
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var userGetAll = await _user.GetUsers();

                if (!UsersValidations.UsersCount(userGetAll, out string messageValid))
                {
                    result.Success = false;
                    result.Message = messageValid;
                    return result;
                }

                result.Data = userGetAll;
                result.Message = "Usuarios obtenidos correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los usuarios.";
                _logger.LogError($"Ha ocurrido un error obteniendo los usuarios: {ex.Message}.");
            }

            return result;
        }

        public async Task<ServiceResult> LoginWithGoogle(string email, string name)
        {

            ServiceResult result = new ServiceResult();

            try
            {
                var userExistent = await _user.GetUserEmail(email);

                if (!UsersValidations.UsersEmailLogin(userExistent, out string messageEmail))
                {
                    result.Success = false;
                    result.Message = messageEmail;
                    return result;
                }

                var userLogin = await _user.GetUserLogin(email, userExistent.PasswordHash, "user");

                result.Data = userLogin;
                result.Message = "Inicio de sesión con Google exitoso.";

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error con el login de Google.";
                _logger.LogError($"Ha ocurrido un error con el login de Google: {ex.Message}");
            }

            return result;

        }

        public async Task<ServiceResult> GetUser(string email, string password, string rol)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                var getUsuarioEmail = await _user.GetUserEmail(email);

                if (!UsersValidations.UsersEmail(getUsuarioEmail, out string messageEmail))
                {
                    result.Success = false;
                    result.Message = messageEmail;
                    return result;
                }

                if (!AuthenticationValidations.AuthenticationValidationPassword(password, getUsuarioEmail.PasswordHash, out string messagePasswordValidation))
                {
                    result.Success = false;
                    result.Message = messagePasswordValidation;
                    return result;
                }

                var getUsuarioNormal = await _user.GetUserLogin(email, getUsuarioEmail.PasswordHash, rol);

                result.Data = getUsuarioNormal;
                result.Message = "Inicio de sesión exitoso.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el usuario.";
                _logger.LogError($"Ha ocurrido un error obteniendo el usuario: {ex.Message}.");
            }

            return result;
        }

        public async Task<ServiceResult> GetUserById(int id)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var userId = await _user.GetUserById(id);

                if (!UsersValidations.UsersVerifyId(userId, out string messageEmail))
                {
                    result.Success = false;
                    result.Message = messageEmail;
                    return result;
                }

                result.Data = userId;
                result.Message = "Usuario Obtenido Correctamente";

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el usuario por el id.";
                _logger.LogError($"Ha ocurrido un error obteniendo el usuario por el id: {ex.Message}");
            }

            return result;

        }

        public async Task<ServiceResult> Add(UsersAddDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                var userExistent = await _user.GetUserEmail(modelDto.Email);

                if (!UsersValidations.UsersEmailExists(userExistent, out string messageEmail))
                {
                    result.Success = false;
                    result.Message = messageEmail;
                    return result;
                }

                if (!UsersValidations.UsersAddValidation(modelDto, out string message))
                {
                    result.Success = false;
                    result.Message = message;
                    return result;
                }

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(modelDto.PasswordHash);
                var urlDecodificada = Uri.UnescapeDataString(modelDto.ImgProfile);

                if (!CloudinaryValidations.IsValidImage(urlDecodificada, out string messageImage))
                {
                    result.Success = false;
                    result.Message = messageImage;
                    return result;
                }

                var imageUrl = await _cloudinaryServices.UploadImageFromUrlAsync(urlDecodificada);

                await _user.Add(new TechTales.Domain.entities.Users
                {

                    ImgProfile = imageUrl,
                    Name = modelDto.Name,
                    Email = modelDto.Email,
                    DateRegister = DateTime.Now,
                    PasswordHash = passwordHash,
                    Role = "user",
                    CreationDate = DateTime.Now,

                });


                await _user.SaveChanges();
                result.Message = "Usuario agregado correctamente";

                var emailModel = this._emailServices.GenerateUsuarioModel(modelDto.Name, modelDto.Email, modelDto.DateRegister, modelDto.PasswordHash);
                var emailBody = this._emailServices.RenderTemplateWelcome("NotificationTemplate", emailModel);
                await _emailServices.SendEmail(modelDto.Email, "¡Bienvenido a TechTales!", emailBody, true);

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el usuario.";
                _logger.LogError($"Ha ocurrido un error guardando el usuario: {ex.Message}.");
            }

            return result;
        }

        public async Task<ServiceResult> AddAdminUser(UsersAddDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                var userExistent = await _user.GetUserEmail(modelDto.Email);

                if (!UsersValidations.UsersEmailExists(userExistent, out string messageEmail))
                {
                    result.Success = false;
                    result.Message = messageEmail;
                    return result;
                }

                if (!UsersValidations.UsersAddValidation(modelDto, out string message))
                {
                    result.Success = false;
                    result.Message = message;
                    return result;
                }

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(modelDto.PasswordHash);
                var urlDecodificada = Uri.UnescapeDataString(modelDto.ImgProfile);

                if (!CloudinaryValidations.IsValidImage(urlDecodificada, out string messageImage))
                {
                    result.Success = false;
                    result.Message = messageImage;
                    return result;
                }

                var imageUrl = await _cloudinaryServices.UploadImageFromUrlAsync(urlDecodificada);

                await _user.Add(new TechTales.Domain.entities.Users
                {

                    ImgProfile = imageUrl,
                    Name = modelDto.Name,
                    Email = modelDto.Email,
                    DateRegister = DateTime.Now,
                    PasswordHash = passwordHash,
                    Role = "admin",
                    CreationDate = DateTime.Now,

                });


                await _user.SaveChanges();
                result.Message = "Usuario agregado correctamente";

                var emailModel = this._emailServices.GenerateUsuarioModel(modelDto.Name, modelDto.Email, modelDto.DateRegister, modelDto.PasswordHash);
                var emailBody = this._emailServices.RenderTemplateWelcome("NotificationTemplate", emailModel);
                await _emailServices.SendEmail(modelDto.Email, "¡Bienvenido a TechTales!", emailBody, true);

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el usuario.";
                _logger.LogError($"Ha ocurrido un error guardando el usuario: {ex.Message}.");
            }

            return result;
        }

        public async Task<ServiceResult> Remove(UsersRemoveDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var userRemove = await _user.GetById(modelDto.Id);

                if (!UsersValidations.UsersVerifyId(userRemove, out string messageVerify))
                {
                    result.Success = false;
                    result.Message = messageVerify;
                    return result;
                }

                if (!UsersValidations.UsersRemoveValidation(modelDto, out string messageValidation))
                {
                    result.Success = false;
                    result.Message = messageValidation;
                    return result;
                }

                userRemove.UserDeleted = modelDto.ChangeUser;

                await _user.Remove(userRemove);
                await _user.SaveChanges();
                result.Message = "Usuario eliminado correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el usuario.";
                _logger.LogError($"Ha ocurrido un error eliminando el usuario: {ex.Message}.");
            }

            return result;
        }

        public async Task<ServiceResult> Update(UsersUpdateDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var userUpdate = await _user.GetById(modelDto.Id);

                if (!UsersValidations.UsersVerifyId(userUpdate, out string messageVerify))
                {
                    result.Success = false;
                    result.Message = messageVerify;
                    return result;
                }

                if (!UsersValidations.UsersUpdateValidation(modelDto, out string messageValidation))
                {
                    result.Success = false;
                    result.Message = messageValidation;
                    return result;
                }

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(modelDto.PasswordHash);


                userUpdate.Name = modelDto.Name;
                userUpdate.Email = modelDto.Email;
                userUpdate.PasswordHash = passwordHash;
                userUpdate.UserMod = modelDto.ChangeUser;
                userUpdate.ModifyDate = DateTime.Now;

                await _user.Update(userUpdate);
                await _user.SaveChanges();
                result.Message = "Usuario actualizado correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el usuario.";
                _logger.LogError($"Ha ocurrido un error actualizando el usuario: {ex.Message}.");
            }

            return result;
        }

        public async Task<ServiceResult> RegisterWithGoogle(UsersAddDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                string passwordTemporal = _passwordHelperServices.GenerarContrasenaTemporal();
                string passwordHash = _passwordHelperServices.HashPassword(passwordTemporal);

                var userExistent = await _user.GetUserEmail(modelDto.Email);

                if (!UsersValidations.UsersEmailExists(userExistent, out string messageEmail))
                {

                    result.Success = false;
                    result.Message = messageEmail;
                    return result;

                }

                var urlDecodificada = Uri.UnescapeDataString(modelDto.ImgProfile);
                var imageUrl = await _cloudinaryServices.UploadImageFromUrlAsync(urlDecodificada);

                if (!CloudinaryValidations.IsValidImage(imageUrl, out string messageImage))
                {
                    result.Success = false;
                    result.Message = messageImage;
                    return result;
                }

                var newUser = new Users
                {
                    ImgProfile = imageUrl,
                    Name = modelDto.Name,
                    Email = modelDto.Email,
                    PasswordHash = passwordHash,
                    DateRegister = DateTime.Now,
                    Role = "usuario",
                    CreationDate = DateTime.Now
                };

                if (!UsersValidations.UsersAddValidationGoogle(newUser, out string mensajeValidacion))
                {
                    result.Success = false;
                    result.Message = mensajeValidacion;
                    return result;
                }

                await _user.Add(newUser);
                await _user.SaveChanges();
                result.Message = "Usuario registrado correctamente";
                result.Data = newUser;

                var emailModel = this._emailServices.GenerateUsuarioModel(newUser.Name, newUser.Email, newUser.DateRegister, passwordTemporal);
                var emailBody = this._emailServices.RenderTemplateWelcome("NotificationTemplate", emailModel);
                await _emailServices.SendEmail(newUser.Email, "¡Bienvenido a TechTales!", emailBody, true);

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al registrar usuario con Google.";
                _logger.LogError($"Error al registrar usuario con Google: {ex.Message}");
                return result;
            }

            return result;

        }

    }
}