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
using Microsoft.EntityFrameworkCore;

namespace TechTales.Persistence.Repositories
{
    public class CommentsRepository : BaseRepository<Comments>, IComments
    {
        
        private readonly DbtechtalesContext dbtechtalesContext;
        private ILogger<CommentsRepository> logger;

        public CommentsRepository(DbtechtalesContext dbtechtalesContext,
            ILogger<CommentsRepository> logger) : base(dbtechtalesContext)
        {
            this.dbtechtalesContext = dbtechtalesContext;
            this.logger = logger;
        }

        public async Task<List<CommentsModel>> GetCommentsByUserId(int usuarioId)
        {
            try
            {
                var comentariosUsuario = await (from cu in dbtechtalesContext.Comments
                                                where !cu.Deleted && cu.IdUser.Equals(usuarioId)
                                                select new CommentsModel()
                                                {
                                                    Id = cu.Id,
                                                    Content = cu.Content,
                                                    DateComment = cu.DateComment,

                                                }).ToListAsync();

                return comentariosUsuario;
            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error obteniendo los comentarios del usuario: {ex.ToString()}.");
                throw new CommentsExceptions("Ha ocurrido un error obteniendo los comentarios del usuario.");
            }
        }

        public async Task<List<CommentsModel>> GetCommentsByStoriesId(int historiaId)
        {
            try
            {
                var comentariosHistoria = await (from cu in dbtechtalesContext.Comments
                                                 where !cu.Deleted && cu.IdStorie.Equals(historiaId)
                                                 select new CommentsModel()
                                                 {
                                                     Id = cu.Id,
                                                     IdStorie = cu.IdStorie,
                                                     IdUser = cu.IdUser,
                                                     Content = cu.Content,
                                                     DateComment = cu.DateComment,

                                                 }).ToListAsync();

                return comentariosHistoria;
            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error obteniendo los comentarios de la historia: {ex.ToString()}.");
                throw new CommentsExceptions("Ha ocurrido un error obteniendo comentarios de la historia.");
            }
        }

        public async Task<int> GetCountCommentsByStoriesId(int historiaId)
        {

            try
            {
                var comentariosCountHistoria = await dbtechtalesContext.Comments
                                            .Where(cu => !cu.Deleted && cu.IdStorie.Equals(historiaId))
                                            .CountAsync();

                return comentariosCountHistoria;
            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error obteniendo el total de comentarios de la historia: {ex.ToString()}.");
                throw new CommentsExceptions("Ha ocurrido un error obteniendo el total de comentarios de la historia.");
            }

        }

        public override async Task Add(Comments entity)
        {
            try
            {
                await base.Add(entity);
                await base.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error guardando el comentario: {ex.ToString()}.");
                throw new CommentsExceptions("Ha ocurrido un error guardando el comentario.");
            }
        }

        public override async Task Remove(Comments entity)
        {
            try
            {
                Comments comentarioRemove = await base.GetById(entity.Id);

                if (comentarioRemove is null)
                {
                    throw new CommentsExceptions("Ha ocurrido un error obteniendo el comentario.");
                }

                comentarioRemove.Deleted = true;
                comentarioRemove.DeletedDate = DateTime.Now;
                comentarioRemove.UserDeleted = entity.UserDeleted;
                comentarioRemove.UserMod = entity.UserMod;

                await base.Update(comentarioRemove);
                await base.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error eliminando el comentario: {ex.ToString()}.");
            }
        }

    }
}
