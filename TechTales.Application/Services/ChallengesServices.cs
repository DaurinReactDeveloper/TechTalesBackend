using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Contract;
using TechTales.Application.Core;
using TechTales.Application.Dtos.RetoDto;
using TechTales.Application.Validations;
using TechTales.Domain.entities;
using TechTales.Persistence.Interfaces;

namespace TechTales.Application.Services
{
    public class ChallengesServices : IChallengesServices
    {
        private readonly IChallenges _challenges;
        private readonly ILogger<ChallengesServices> logger;

        public ChallengesServices(IChallenges challenges, ILogger<ChallengesServices> logger)
        {
            this._challenges = challenges;
            this.logger = logger;
        }

        public async Task<ServiceResult> GetChallengesForType(string type)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var challengesGetAll = await _challenges.GetChallengesForType(type);

                if (!ChallengesValidations.ChallengesCount(challengesGetAll, out string messageCount))
                {
                    result.Success = false;
                    result.Message = messageCount;
                    return result;
                }

                result.Data = challengesGetAll;
                result.Message = "Retos obtenidos correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los retos.";
                this.logger.LogError($"Ha ocurrido un error obteniendo los retos: {ex.Message}.");
            }

            return result;
        }

        public async Task<ServiceResult> GetChallenges()
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var challengesGetAll = await _challenges.GetChallenges();

                if (!ChallengesValidations.ChallengesCount(challengesGetAll, out string messageCount))
                {
                    result.Success = false;
                    result.Message = messageCount;
                    return result;
                }

                result.Data = challengesGetAll;
                result.Message = "Retos obtenidos correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los retos.";
                this.logger.LogError($"Ha ocurrido un error obteniendo los retos: {ex.Message}.");
            }

            return result;
        }

        public async Task<ServiceResult> Add(ChallengesAddDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                if (!ChallengesValidations.ChallengesAddValidation(modelDto, out string message))
                {
                    result.Success = false;
                    result.Message = message;
                    return result;
                }

                var challengesAdd = new Domain.entities.Challenges
                {
                    Id = modelDto.Id,
                    Title = modelDto.Title,
                    Type = modelDto.Type,
                    Description = modelDto.Description,
                    Hint = modelDto.Hint,
                    IdAdmin = modelDto.IdAdmin,
                    DatePublication = DateTime.Now,
                    CreationDate = DateTime.Now,
                    CreationUser = modelDto.ChangeUser,
                };

                await _challenges.Add(challengesAdd);
                await _challenges.SaveChanges();
                result.Message = "Reto agregado correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el reto.";
                logger.LogError($"Ha ocurrido un error guardando el reto: {ex.Message}.");
            }

            return result;
        }

        public async Task<ServiceResult> Remove(ChallengesRemoveDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var challengesRemove = await _challenges.GetById(modelDto.Id);

                if (!ChallengesValidations.ChallengesVerifyId(challengesRemove, out string messageVerify))
                {
                    result.Success = false;
                    result.Message = messageVerify;
                    return result;
                }

                if (!ChallengesValidations.ChallengesRemoveValidation(modelDto, out string messageValidation))
                {
                    result.Success = false;
                    result.Message = messageValidation;
                    return result;
                }

                challengesRemove.UserDeleted = modelDto.ChangeUser;

                await _challenges.Remove(challengesRemove);
                await _challenges.SaveChanges();
                result.Message = "Reto eliminado correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el reto.";
                logger.LogError($"Ha ocurrido un error eliminando el reto: {ex.Message}.");
            }

            return result;
        }

        public async Task<ServiceResult> Update(ChallengesUpdateDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var challengesUpdate = await _challenges.GetById(modelDto.Id);

                if (!ChallengesValidations.ChallengesVerifyId(challengesUpdate, out string messageVerify))
                {
                    result.Success = false;
                    result.Message = messageVerify;
                    return result;
                }

                if (!ChallengesValidations.ChallengesUpdateValidation(modelDto, out string messageValidation))
                {
                    result.Success = false;
                    result.Message = messageValidation;
                    return result;
                }

                challengesUpdate.Title = modelDto.Title;
                challengesUpdate.Type = modelDto.Type;
                challengesUpdate.Description = modelDto.Description;
                challengesUpdate.Hint = modelDto.Hint;
                challengesUpdate.UserMod = modelDto.ChangeUser;
                challengesUpdate.ModifyDate = DateTime.Now;

                await _challenges.Update(challengesUpdate);
                await _challenges.SaveChanges();
                result.Message = "Reto actualizado correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el reto.";
                logger.LogError($"Ha ocurrido un error actualizando el reto: {ex.Message}.");
            }

            return result;
        }
    
    }
}