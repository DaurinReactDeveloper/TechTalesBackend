using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Dtos.UsuarioDto;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Models;
using TechTales.Persistence.Interfaces;

namespace TechTales.Application.Validations
{
    public static class UsersValidations
    {

        public static bool UsersAddValidation(UsersAddDto userAdd, out string message)
        {

            message = string.Empty;

            if (userAdd is null)
            {
                message = "El usuario no puede ser nulo.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(userAdd.Name) || userAdd.Name.Length <= 8 || userAdd.Name.Length >= 15)
            {
                message = "El nombre debe tener al menos 9 caracteres y no puede estar vacío.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(userAdd.Email) || !IsValidEmail(userAdd.Email) || userAdd.Email.Length <= 10 || userAdd.Email.Length >= 49)
            {
                message = "El correo electrónico no es válido o está vacío.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(userAdd.PasswordHash) || userAdd.PasswordHash.Length < 8 || !HasNumberAndLetter(userAdd.PasswordHash) || userAdd.PasswordHash.Length >= 15)
            {
                message = "La contraseña debe tener al menos 8 caracteres y contener tanto letras como números.";
                return false;
            }

            return true;
        }

        public static bool UsersAddValidationGoogle(Users userAdd, out string message)
        {

            message = string.Empty;

            if (userAdd is null)
            {
                message = "El usuario no puede ser nulo.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(userAdd.Name) || userAdd.Name.Length < 9 || userAdd.Name.Length >= 14)
            {
                message = "El nombre debe tener al menos 3 caracteres y no puede estar vacío.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(userAdd.Email) || !IsValidEmail(userAdd.Email) || userAdd.Email.Length <= 10 || userAdd.Email.Length >= 49)
            {
                message = "El correo electrónico no es válido o está vacío.";
                return false;
            }

            return true;
        }

        public static bool UsersUpdateValidation(UsersUpdateDto userUpdateDto, out string message)
        {

            message = string.Empty;

            if (userUpdateDto.Id <= 0)
            {
                message = "No se pudo obtener el usuario.";
                return false;

            }

            if (userUpdateDto is null)
            {
                message = "El usuario no puede ser nulo.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(userUpdateDto.Name) || userUpdateDto.Name.Length < 9 || userUpdateDto.Name.Length >= 14)
            {

                message = "El nombre debe tener al menos 3 caracteres y no puede estar vacío.";
                return false;

            }

            if (string.IsNullOrWhiteSpace(userUpdateDto.Email) || !IsValidEmail(userUpdateDto.Email))
            {
                message = "El correo electrónico no es válido o está vacío.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(userUpdateDto.PasswordHash) || userUpdateDto.PasswordHash.Length < 8 || !HasNumberAndLetter(userUpdateDto.PasswordHash))
            {
                message = "La contraseña debe tener al menos 8 caracteres y contener tanto letras como números.";
                return false;
            }

            return true;

        }

        public static bool UsersRemoveValidation(UsersRemoveDto userRemove, out string message)
        {

            message= string.Empty;

            if (userRemove.Id <= 0)
            {
                message = "No se pudo obtener el usuario.";
                return false;

            }

            return true;

        }

        private static bool IsValidEmail(string email)
        {
            var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        private static bool HasNumberAndLetter(string password)
        {
            bool hasLetter = false;
            bool hasNumber = false;

            foreach (char c in password)
            {
                if (char.IsLetter(c)) hasLetter = true;
                if (char.IsDigit(c)) hasNumber = true;
            }

            return hasLetter && hasNumber;

        }

        public static bool UsersVerifyId(Users userId, out string message)
        {
            message = string.Empty;

            if (userId is null)
            {
                message = "No se pudo obtener el Usuario.";
                return false;
            }

            return true;

        }

        public static bool UsersVerifyId(UsersModel userId, out string message)
        {
            message = string.Empty;

            if (userId is null)
            {
                message = "No se pudo obtener el Usuario.";
                return false;
            }

            return true;

        }

        public static bool UsersCount(List<UsersModel> users, out string message)
        {

            message = string.Empty;

            if (users.Count <= 0)
            {
                message = "No se encontraron los usuarios disponibles.";
                return false;

            }


            return true;

        }

        public static bool UsersEmail(UsersModel user, out string message)
        {

            message = string.Empty;

            if (user is null)
            {
                message = "Email Incorrecto";
                return false;
                     
            }

            return true;
        }

        public static bool UsersEmailLogin(UsersModel user, out string message)
        {

            message = string.Empty;

            if (user is null)
            {
                message = "El usuario no se encuentra registrado. Por favor, diríjase a la sección de registro para crear una cuenta.";
                return false;

            }

            return true;
        }

        public static bool UsersEmailExists(UsersModel user, out string message)
        {
            message = string.Empty;

            if (user != null) 
            {
                message = "El correo electrónico ya está registrado.";
                return false; 
            }

            return true; 
        }

    }
}
