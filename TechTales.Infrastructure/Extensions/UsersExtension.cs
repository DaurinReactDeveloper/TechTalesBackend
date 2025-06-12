using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Models;

namespace TechTales.Infrastructure.Extensions
{
    public static class UsersExtension
    {

        public static UsersModel ConvertUsuarioEntityToModel(this Users usuarioEntity)
        {

            var usuarioModel = new UsersModel()
            {

                Id = usuarioEntity.Id,
                ImgProfile = usuarioEntity.ImgProfile,
                Name = usuarioEntity.Name,
                Email = usuarioEntity.Email,
                PasswordHash = usuarioEntity.PasswordHash,
                Role = usuarioEntity.Role,
                DateRegister = usuarioEntity.DateRegister,

            };

            return usuarioModel;   

        }

    }
}
