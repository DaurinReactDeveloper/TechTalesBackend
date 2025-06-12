using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Exceptions;
using TechTales.Infrastructure.Models;
using TechTales.Persistence.Context;
using TechTales.Persistence.Core;
using TechTales.Persistence.Interfaces;

namespace TechTales.Persistence.Repositories
{
    public class UsersRepository : BaseRepository<Users>, IUsers
    {

        private readonly DbtechtalesContext dbtechtalesContext;
        private readonly ILogger<UsersRepository> logger;

        public UsersRepository(DbtechtalesContext dbtechtalesContext, ILogger<UsersRepository> logger) : base(dbtechtalesContext)
        {

            this.dbtechtalesContext = dbtechtalesContext;
            this.logger = logger;

        }

        public async Task<List<UsersModel>> GetUsers()
        {
            try
            {
                var usuarioModel = await (from um in dbtechtalesContext.Users
                                          where !um.Deleted
                                          select new UsersModel()
                                          {
                                              Id = um.Id,
                                              Name = um.Name,
                                              Email = um.Email,
                                              PasswordHash = um.PasswordHash,
                                              DateRegister = um.DateRegister,
                                          }).ToListAsync();

                return usuarioModel;

            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error obteniendo los usuarios, {ex.ToString()}.");
                throw new UsersExcepetions("Ha ocurrido un error obteniendo los usuarios.");
            }
        }

        public async Task<UsersModel> GetUserEmail(string email)
        {
            try
            {
                var usuario = await (from us in dbtechtalesContext.Users
                                     where !us.Deleted && us.Email.Equals(email)
                                     select new UsersModel()
                                     {
                                         Email = us.Email,
                                         PasswordHash = us.PasswordHash,

                                     }).FirstOrDefaultAsync();

                return usuario;

            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error obteniendo el usuario, {ex.ToString()}.");
                throw new UsersExcepetions("Ha ocurrido un error obteniendo el usuario.");
            }
        }

        public async Task<UsersModel> GetUserLogin(string email, string password, string rol)
        {
            try
            {
                var usuario = await (from us in dbtechtalesContext.Users
                                     where !us.Deleted && us.Email.Equals(email) && us.PasswordHash.Equals(password) && us.Role.Equals(rol)
                                     select new UsersModel()
                                     {
                                         Id = us.Id,
                                         ImgProfile = us.ImgProfile,
                                         Name = us.Name,
                                         Email = us.Email,
                                         Role = us.Role,

                                     }).FirstOrDefaultAsync();

                return usuario;

            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error obteniendo el usuario, {ex.ToString()}.");
                throw new UsersExcepetions("Ha ocurrido un error obteniendo el usuario.");
            }
        }

        public async Task<UsersModel> GetUserById(int id)
        {
            try
            {
                var usuario = await (from us in dbtechtalesContext.Users
                                     where !us.Deleted && us.Id.Equals(id)
                                     select new UsersModel()
                                     {

                                         ImgProfile = us.ImgProfile,
                                         Name = us.Name,


                                     }).FirstOrDefaultAsync();

                return usuario;

            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error obteniendo el usuario por el id, {ex.ToString()}.");
                throw new UsersExcepetions("Ha ocurrido un error obteniendo el usuario id.");
            }
        }

        public override async Task Add(Users entity)
        {
            try
            {
                await base.Add(entity);
                await base.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error guardando el usuario, {ex.ToString()}.");
                throw new UsersExcepetions("Ha ocurrido un error guardando el usuario.");
            }
        }

        public override async Task Update(Users entity)
        {
            try
            {
                var usuarioUpdate = await base.GetById(entity.Id);

                if (usuarioUpdate is null)
                {
                    throw new UsersExcepetions("Ha ocurrido un error obteniendo el usuario.");
                }

                usuarioUpdate.Name = entity.Name;
                usuarioUpdate.Email = entity.Email;
                usuarioUpdate.PasswordHash = entity.PasswordHash;
                usuarioUpdate.ModifyDate = entity.ModifyDate;
                usuarioUpdate.UserMod = entity.UserMod;

                await base.Update(usuarioUpdate);
                await base.SaveChanges();

            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error actualizando el usuario, {ex.ToString()}.");
            }
        }

        public override async Task Remove(Users entity)
        {
            try
            {
                var usuarioRemove = await base.GetById(entity.Id);

                if (usuarioRemove is null)
                {
                    throw new UsersExcepetions("Ha ocurrido un error obteniendo el usuario.");
                }

                usuarioRemove.Deleted = true;
                usuarioRemove.DeletedDate = DateTime.Now;
                usuarioRemove.UserDeleted = entity.UserDeleted;

                await base.Update(usuarioRemove);
                await base.SaveChanges();

            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error eliminando el usuario, {ex.ToString()}.");
            }
        }

    }
}
