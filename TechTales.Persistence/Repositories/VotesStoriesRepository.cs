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
    public class VotesStoriesRepository : BaseRepository<VotesStories>, IVotesStories
    {
        private readonly DbtechtalesContext dbtechtalesContext;
        private readonly ILogger<VotesStoriesRepository> logger;

        public VotesStoriesRepository(DbtechtalesContext dbtechtalesContext, ILogger<VotesStoriesRepository> logger) : base(dbtechtalesContext)
        {
            this.dbtechtalesContext = dbtechtalesContext;
            this.logger = logger;
        }

        public async Task<(int likes, int dislikes)> GetVotesByStoriesId(int idHistoria)
        {

            try
            {
                var votos = await dbtechtalesContext.VotesStories
           .Where(v => v.IdStories == idHistoria && !v.Deleted)
           .ToListAsync();

                int likes = votos.Count(v => v.Vote);
                int dislikes = votos.Count(v => !v.Vote);
                return (likes, dislikes);
            }

            catch (Exception ex)
            {
                logger.LogError("Error al verificar al conseguir los votos de la historia: " + ex.Message);
                throw new VotesStoriesExceptions("Ha ocurrido un error al conseguir los votos de la historia.");
            }

        }

        public async Task<VotesStories> GetVotesByUserAndStories(int idUsuario, int idHistoria)
        {

            try
            {

                return await dbtechtalesContext.VotesStories
                        .FirstOrDefaultAsync(v => v.IdUser == idUsuario && v.IdStories == idHistoria && v.Deleted == false);

            }
            catch (Exception ex)
            {
                logger.LogError("Error al verificar la historia del usuario: " + ex.Message);
                throw new VotesStoriesExceptions("Ha ocurrido un error al verificar la historia del usuario.");
            }

        }

        public async Task<VotesStories> GetVotesByUserAndStoriesTrueDelete(int userId, int storyId)
        {

            try
            {
                return await dbtechtalesContext.VotesStories
          .FirstOrDefaultAsync(x => x.IdUser == userId && x.IdStories == storyId);
            }
            catch (Exception ex)
            {
                logger.LogError("Error al verificar la historia del usuario: " + ex.Message);
                throw new VotesStoriesExceptions("Ha ocurrido un error al verificar la historia del usuario.");
            }
        }

        public override async Task Add(VotesStories entity)
        {
            try
            {
                await base.Add(entity);
                await base.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error guardando el voto de la historia, {ex.ToString()}.");
                throw new VotesStoriesExceptions("Ha ocurrido un error guardando el voto de la historia.");
            }
        }

        public override async Task Update(VotesStories entity)
        {
            try
            {
                var votosHistoriaUpdate = await base.GetById(entity.Id);

                if (votosHistoriaUpdate == null)
                {
                    throw new VotesStoriesExceptions("Ha ocurrido un error obteniendo el voto de la historia.");
                }

                votosHistoriaUpdate.Vote = entity.Vote;
                votosHistoriaUpdate.ModifyDate = DateTime.Now;
                votosHistoriaUpdate.UserMod = entity.UserMod;
                votosHistoriaUpdate.Deleted = false;

                await base.Update(votosHistoriaUpdate);
                await base.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error actualizando el voto de la historia, {ex.ToString()}.");
            }
        }

        public override async Task Remove(VotesStories entity)
        {
            try
            {
                var votosHistoriaRemove = await base.GetById(entity.Id);

                if (votosHistoriaRemove == null)
                {
                    throw new VotesStoriesExceptions("Ha ocurrido un error obteniendo el voto de la historia.");
                }

                votosHistoriaRemove.Deleted = true;
                votosHistoriaRemove.UserDeleted = entity.UserDeleted;
                votosHistoriaRemove.DeletedDate = DateTime.Now;

                await base.Update(votosHistoriaRemove);
                await base.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error eliminando el voto de la historia, {ex.ToString()}.");
            }
        }

    }
}
