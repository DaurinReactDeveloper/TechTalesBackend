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
    public class ChallengesCompletedRepository : BaseRepository<ChallengesCompleted>, IChallengesCompleted
    {

        private readonly DbtechtalesContext dbtechtalesContext;
        private readonly ILogger<ChallengesCompletedRepository> logger;

        public ChallengesCompletedRepository(DbtechtalesContext dbtechtalesContext, ILogger<ChallengesCompletedRepository> logger) : base(dbtechtalesContext)
        {
            this.dbtechtalesContext = dbtechtalesContext;
            this.logger = logger;
        }

        public async Task<List<ChallengesCompletedModel>> GetChallengesCompletedByUserId(int userId)
        {
            try
            {

                var retosCompletados = await (from rc in dbtechtalesContext.ChallengesCompleted
                                              join ch in dbtechtalesContext.Challenges on rc.IdChallenges equals ch.Id
                                              where !rc.Deleted && rc.IdUser.Equals(userId)
                                              select new ChallengesCompletedModel()
                                              {
                                                  Id = rc.Id,
                                                  IdUser = rc.IdUser,
                                                  IdChallenges = rc.IdChallenges,
                                                  DateCompleted = rc.DateCompleted,
                                                  IdChallengesNavigation = ch 

                                              }).ToListAsync();

                return retosCompletados;
            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error obteniendo los retos completados del usuario, {ex.ToString()}.");
                throw new ChallengesCompletedExceptions("Ha ocurrido un error obteniendo los retos completados del usuario.");
            }
        }

        public async Task<bool> VerifyChallengesCompleted(int idChallenges, int idUser)
        {
            try
            {

                var retoCompletado = await dbtechtalesContext.ChallengesCompleted
                 .AsNoTracking()
                 .AnyAsync(rc => !rc.Deleted && rc.IdUser == idUser && rc.IdChallenges == idChallenges);

                return retoCompletado;

            }
            catch (Exception ex)
            {

                logger.LogError($"Ha ocurrido un error verificando el reto completado, {ex.ToString()}.");
                throw new ChallengesCompletedExceptions("Ha ocurrido un error verificando el reto completado.");
            }

        }

        public async Task<ChallengesCompleted?> GetDeletedChallenge(int idChallenges, int idUser)
        {
            try
            {
                return await dbtechtalesContext.ChallengesCompleted
                    .Where(x => x.IdChallenges == idChallenges && x.IdUser == idUser && x.Deleted)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error al buscar reto eliminado: {ex}");
                throw new ChallengesCompletedExceptions("Error al buscar reto eliminado.");
            }
        }

        public override async Task Add(ChallengesCompleted entity)
        {
            try
            {
                await base.Add(entity);
                await base.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error guardando el reto completado, {ex.ToString()}.");
                throw new ChallengesCompletedExceptions("Ha ocurrido un error guardando el reto completado.");
            }
        }

        public override async Task Remove(ChallengesCompleted entity)
        {
            try
            {
                var retosCompletadosRemove = await base.GetById(entity.Id);

                if (retosCompletadosRemove is null)
                {
                    throw new ChallengesCompletedExceptions("Ha ocurrido un error obteniendo el reto completado.");
                }

                retosCompletadosRemove.Deleted = true;
                retosCompletadosRemove.DeletedDate = DateTime.Now;
                retosCompletadosRemove.UserDeleted = entity.UserDeleted;

                await base.Update(retosCompletadosRemove);
                await base.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error eliminando el reto completado, {ex.ToString()}.");
            }
        }

    }
}
