using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public class ChallengesRepository : BaseRepository<Challenges>, IChallenges
    {
        private readonly DbtechtalesContext dbtechtalesContext;
        private readonly ILogger<ChallengesRepository> logger;

        public ChallengesRepository(DbtechtalesContext dbtechtalesContext, ILogger<ChallengesRepository> logger) : base(dbtechtalesContext)
        {
            this.logger = logger;
            this.dbtechtalesContext = dbtechtalesContext;
        }

        public async Task<List<ChallengesModel>> GetChallengesForType(string type)
        {
            try
            {
                //Func<Challenges, bool> filter = rm => !rm.Deleted && rm.Type.Equals(type);
                //- No es recomendable usar Func en consultas de EF Core, ya que no se puede traducir a SQL.
                // Usar Expression<Func<Challenges, bool>> para que EF Core pueda traducirlo a SQL

                Expression<Func<Challenges,bool>> filter = rm => !rm.Deleted && rm.Type.Equals(type);

                var retos = await dbtechtalesContext.Challenges
                    .Where(filter)
                    .Select(rm => new ChallengesModel
                    {

                        Id = rm.Id,
                        Title = rm.Title,
                        Description = rm.Description,
                        DatePublication = rm.DatePublication,
                        Hint = rm.Hint
                       
                    }).ToListAsync();

                return retos;
            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error obteniendo los retos por tipos, {ex.ToString()}.");
                throw new ChallengesExceptions("Ha ocurrido un error obteniendo los retos por tipos.");
            }
        }

        public async Task<List<ChallengesModel>> GetChallenges()
        {
            try
            {
                var retos = await dbtechtalesContext.Challenges
                    .Where(rm => !rm.Deleted)
                    .Select(rm => new ChallengesModel
                    {

                        Id = rm.Id,
                        Title = rm.Title,
                        Description = rm.Description,
                        Type = rm.Type,
                        DatePublication = rm.DatePublication,
                        Hint = rm.Hint
                        

                    })
                    .ToListAsync();

                return retos;
            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error obteniendo los retos, {ex.ToString()}.");
                throw new ChallengesExceptions("Ha ocurrido un error obteniendo los retos.");
            }
        }

        public override async Task Add(Challenges entity)
        {
            try
            {
                await base.Add(entity);
                await base.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error guardando el reto, {ex.ToString()}.");
                throw new ChallengesExceptions("Ha ocurrido un error guardando el reto.");
            }
        }

        public override async Task Update(Challenges entity)
        {
            try
            {
                Challenges retoUpdate = await base.GetById(entity.Id);

                if (retoUpdate is null)
                {
                    throw new ChallengesExceptions("Ha ocurrido un error obteniendo el reto.");
                }

                retoUpdate.Title = entity.Title;
                retoUpdate.Type = entity.Type;
                retoUpdate.Description = entity.Description;
                retoUpdate.Hint = entity.Hint;
                retoUpdate.ModifyDate = DateTime.Now;
                retoUpdate.UserMod = entity.UserMod;

                await base.Update(retoUpdate);
                await base.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error actualizando el reto, {ex.ToString()}.");
            }
        }

        public override async Task Remove(Challenges entity)
        {
            try
            {
                Challenges retoRemove = await base.GetById(entity.Id);

                if (retoRemove is null)
                {
                    throw new ChallengesExceptions("Ha ocurrido un error obteniendo el reto.");
                }

                retoRemove.Deleted = true;
                retoRemove.DeletedDate = DateTime.Now;
                retoRemove.UserDeleted = entity.UserDeleted;

                await base.Update(retoRemove);
                await base.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError($"Ha ocurrido un error eliminando el reto, {ex.ToString()}.");
            }
        }
   
    }
}