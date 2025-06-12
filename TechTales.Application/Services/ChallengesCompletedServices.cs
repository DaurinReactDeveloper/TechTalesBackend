using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Contract;
using TechTales.Application.Core;
using TechTales.Application.Dtos.RetosCompletados;
using TechTales.Application.Validations;
using TechTales.Domain.entities;
using TechTales.Persistence.Interfaces;

namespace TechTales.Application.Services
{
    public class ChallengesCompletedServices : IChallengesCompletedServices
    {

        private readonly IChallengesCompleted _challengesCompleted;
        private readonly ILogger<ChallengesCompletedServices> logger;

        public ChallengesCompletedServices(IChallengesCompleted challengesCompleted, ILogger<ChallengesCompletedServices> logger)
        {
            this._challengesCompleted = challengesCompleted;
            this.logger = logger;
        }

        public async Task<ServiceResult> GetChallengesCompletedByUserId(int userId)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                var challengesCompletedByUserId = await _challengesCompleted.GetChallengesCompletedByUserId(userId);

                if (!ChallengesCompletedValidations.ChallengesCount(challengesCompletedByUserId, out string messageCount))
                {
                    result.Success = false;
                    result.Message = messageCount;
                    return result;
                }

                result.Data = challengesCompletedByUserId;
                result.Message = "Retos Completados del usuario obtenidos correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los retos completados del usuario.";
                this.logger.LogError($"Ha ocurrido un error obteniendo los retos completados del usuario: {ex.Message}.");
            }

            return result;
        }

        public async Task<ServiceResult> Add(ChallengesCompletedAddDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {

                var verifyChallenged = await _challengesCompleted.VerifyChallengesCompleted(modelDto.IdChallenges, modelDto.IdUser);

                if (verifyChallenged)
                {
                    result.Success = false;
                    result.Message = "Este reto ya ha sido marcado como completado anteriormente. No es posible marcarlo nuevamente.";
                    return result;
                }

                if (!ChallengesCompletedValidations.ChallengesCompletadosAddValidation(modelDto, out string messageValidation))
                {
                    result.Success = false;
                    result.Message = messageValidation;
                    return result;
                }

                var deletedChallenge = await _challengesCompleted.GetDeletedChallenge(modelDto.IdChallenges, modelDto.IdUser);

                if (deletedChallenge != null)
                {
                    deletedChallenge.Deleted = false;
                    deletedChallenge.DeletedDate = null;
                    deletedChallenge.DateCompleted = DateTime.Now;
                    deletedChallenge.CreationDate = DateTime.Now;
                    deletedChallenge.CreationUser = modelDto.ChangeUser;

                    await _challengesCompleted.Update(deletedChallenge);
                    await _challengesCompleted.SaveChanges();

                    result.Message = "Reto Completado reactivado correctamente";
                }
                else
                {

                    var challengesCompletedAdd = new Domain.entities.ChallengesCompleted()
                    {
                        Id = modelDto.Id,
                        IdChallenges = modelDto.IdChallenges,
                        IdUser = modelDto.IdUser,
                        DateCompleted = DateTime.Now,
                        CreationDate = DateTime.Now,
                        CreationUser = modelDto.ChangeUser
                    };

                    await _challengesCompleted.Add(challengesCompletedAdd);
                    await _challengesCompleted.SaveChanges();

                    result.Message = "Reto Completado agregado correctamente";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el reto completado.";
                logger.LogError($"Ha ocurrido un error guardando el reto completado: {ex.Message}.");
            }

            return result;
        }

        public async Task<ServiceResult> Remove(ChallengesCompletedRemoveDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var challengesCompletedRemove = await _challengesCompleted.GetById(modelDto.Id);

                if (!ChallengesCompletedValidations.ChallengesCompletadosVerifyId(challengesCompletedRemove, out string messageVerify))
                {
                    result.Success = false;
                    result.Message = messageVerify;
                    return result;
                }

                if (!ChallengesCompletedValidations.ChallengesCompletadosRemoveValidation(modelDto, out string messageValidation))
                {
                    result.Success = false;
                    result.Message = messageValidation;
                    return result;
                }

                challengesCompletedRemove.UserDeleted = modelDto.ChangeUser;

                await _challengesCompleted.Remove(challengesCompletedRemove);
                await _challengesCompleted.SaveChanges();

                result.Message = "Reto Completado eliminado correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el reto completado.";
                logger.LogError($"Ha ocurrido un error eliminando el reto completado: {ex.Message}.");
            }

            return result;
        }
    }
}
