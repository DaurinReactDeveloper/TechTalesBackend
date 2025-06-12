using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Exceptions;
using TechTales.Infrastructure.Models;
using TechTales.Persistence.Context;
using TechTales.Persistence.Core;
using TechTales.Persistence.Interfaces;

namespace TechTales.Persistence.Repositories
{
    public class StoriesRepository : BaseRepository<Stories>, IStories
    {

        private readonly DbtechtalesContext dbtechtalesContext;
        private readonly ILogger<Stories> logger;

        public StoriesRepository(DbtechtalesContext dbtechtalesContext, ILogger<Stories> logger) : base(dbtechtalesContext)
        {

            this.dbtechtalesContext = dbtechtalesContext;
            this.logger = logger;

        }

        public async Task<List<StoriesModel>> GetStoriesByUserId(int usuarioId)
        {
            try
            {
                var historias = await (from hs in dbtechtalesContext.Stories
                                       where !hs.Deleted && hs.IdUser.Equals(usuarioId)
                                       select new StoriesModel()
                                       {
                                           Id = hs.Id,
                                           Title = hs.Title,
                                           Content = hs.Content,
                                           DatePublication = hs.DatePublication,
                                           IdUser = hs.IdUser,

                                       }).ToListAsync();

                return historias;
            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error obteniendo las historias del usuario,{ex.ToString()}.");
                throw new StoriesExceptions("Ha ocurrido un error obteniendo las historias del usuario.");
            }
        }
        
        public async Task<List<StoriesModel>> GetStories()
        {
            try
            {
                var historias = await (from hs in dbtechtalesContext.Stories
                                       where !hs.Deleted
                                       orderby hs.DatePublication descending
                                       select new StoriesModel()
                                       {
                                           Id = hs.Id,
                                           Title = hs.Title,
                                           Content = hs.Content,
                                           DatePublication = hs.DatePublication,
                                           IdUser = hs.IdUser,

                                       }).ToListAsync();

                return historias;
            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error obteniendo las historias,{ex.ToString()}.");
                throw new StoriesExceptions("Ha ocurrido un error obteniendo las historias.");
            }
        }

        public override async Task Add(Stories entity)
        {
            try
            {

               await base.Add(entity);
               await base.SaveChanges();

            }
            catch (Exception ex)
            {

                logger.LogError($"Ha ocurrido un error guardando el comentario: {ex.ToString()}.");
                throw new StoriesExceptions("Ha ocurrido un error guardando la historia.");
            
            }

        }

        public override async Task Update(Stories entity)
        {
            try
            {

                 Stories historiaUpdate = await base.GetById(entity.Id);

                if (historiaUpdate is null)
                {

                    throw new StoriesExceptions("Ha ocurrido un error obteniendo la historia.");

                }

                historiaUpdate.Title = entity.Title;
                historiaUpdate.Content = entity.Content;
                historiaUpdate.ModifyDate = DateTime.Now;
                historiaUpdate.UserMod = entity.UserMod;

                await base.Update(historiaUpdate);
                await base.SaveChanges();

            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error actualizando la historia, {ex.ToString()}.");

            }

        }

        public override async Task Remove(Stories entity)
        {

            try
            {

                Stories historiaRemove = await base.GetById(entity.Id);

                if (historiaRemove is null)
                {

                    throw new StoriesExceptions("Ha ocurrido un error obteniendo la historia.");

                }

                historiaRemove.Deleted = true;
                historiaRemove.DeletedDate = DateTime.Now;
                historiaRemove.UserDeleted = entity.UserDeleted;

                await base.Update(historiaRemove);
                await base.SaveChanges();

            }
            catch (Exception ex)
            {

                logger.LogError($"Ha ocurrido un error eliminando la historia, {ex.ToString()}.");
            }



        }

    }
}
