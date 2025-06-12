using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechTales.Application.Contract;
using TechTales.Application.Core;
using TechTales.Application.Dtos.HistoriaDto;
using TechTales.Application.Validations;
using TechTales.Domain.entities;
using TechTales.Persistence.Interfaces;

namespace TechTales.Application.Services
{
    public class StoriesServices : IStoriesServices
    {
        private readonly IStories _stories;
        private readonly ILogger<StoriesServices> _logger;

        public StoriesServices(IStories stories, ILogger<StoriesServices> logger)
        {
            this._stories = stories;
            this._logger = logger;
        }

        public async Task<ServiceResult> GetStories()
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var storiesGetAll = await _stories.GetStories();

                if (!StoriesValidations.StoriesCount(storiesGetAll, out string messageCount))
                {
                    result.Success = false;
                    result.Message = messageCount;
                    return result;
                }

                result.Data = storiesGetAll;
                result.Message = "Historias obtenidas correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo las historias.";
                this._logger.LogError($"Ha ocurrido un error obteniendo las historias: {ex.Message}.");
            }

            return result;
        }

        public async Task<ServiceResult> GetStoriesByUserId(int userId)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var stories = await this._stories.GetStoriesByUserId(userId);

                if (!StoriesValidations.StoriesCount(stories, out string messageCount))
                {
                    result.Success = false;
                    result.Message = messageCount;
                    return result;
                }

                result.Data = stories;
                result.Message = "Historias del usuario obtenidas correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo las historias del usuario.";
                this._logger.LogError($"Ha ocurrido un error obteniendo las historias del usuario: {ex.Message}.");
            }

            return result;
        }

        public async Task<ServiceResult> Add(StoriesAddDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                if (!StoriesValidations.StoriesAddValidation(modelDto, out string messageValidation))
                {
                    result.Success = false;
                    result.Message = messageValidation;
                    return result;
                }

                var storieAdd = new Domain.entities.Stories()
                {
                    Id = modelDto.Id,
                    Title = modelDto.Title,
                    Content = modelDto.Content,
                    DatePublication = DateTime.Now,
                    IdUser = modelDto.IdUser,
                    CreationUser = modelDto.ChangeUser
                };

                await _stories.Add(storieAdd);
                await _stories.SaveChanges();
                result.Message = "Historia agregada correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando la historia.";
                _logger.LogError($"Ha ocurrido un error guardando la historia: {ex.Message}.");
            }

            return result;
        }

        public async Task<ServiceResult> Remove(StoriesRemoveDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var storiesRemove = await _stories.GetById(modelDto.Id);
                
                if(!StoriesValidations.StoriesPermits(modelDto,storiesRemove, out string messageValidPermits))
                {
                    result.Success = false;
                    result.Message = messageValidPermits;
                    return result;
                }

                if (!StoriesValidations.StoriesVerifyId(storiesRemove, out string messageVerify))
                {
                    result.Success = false;
                    result.Message = messageVerify;
                    return result;
                }

                if (!StoriesValidations.StoriesRemoveValidation(modelDto, out string messageValidation))
                {
                    result.Success = false;
                    result.Message = messageValidation;
                    return result;
                }

                storiesRemove.UserDeleted = modelDto.ChangeUser;

                await _stories.Remove(storiesRemove);
                await _stories.SaveChanges();
                result.Message = "Historia eliminada correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando la historia.";
                _logger.LogError($"Ha ocurrido un error eliminando la historia: {ex.Message}.");
            }

            return result;
        }

        public async Task<ServiceResult> Update(StoriesUpdateDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var storiesUpdate = await _stories.GetById(modelDto.Id);

                if (!StoriesValidations.StoriesPermits(modelDto, storiesUpdate, out string messageValidPermits))
                {
                    result.Success = false;
                    result.Message = messageValidPermits;
                    return result;
                }

                if (!StoriesValidations.StoriesVerifyId(storiesUpdate, out string messageVerify))
                {
                    result.Success = false;
                    result.Message = messageVerify;
                    return result;
                }

                if (!StoriesValidations.StoriesUpdateValidation(modelDto, out string messageValidation))
                {
                    result.Success = false;
                    result.Message = messageValidation;
                    return result;
                }

                storiesUpdate.Title = modelDto.Title;
                storiesUpdate.Content = modelDto.Content;
                storiesUpdate.ModifyDate = DateTime.Now;
                storiesUpdate.UserMod = modelDto.ChangeUser;

                await _stories.Update(storiesUpdate);
                await _stories.SaveChanges();
                result.Message = "Historia actualizada correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando la historia.";
                _logger.LogError($"Ha ocurrido un error actualizando la historia: {ex.Message}.");
            }

            return result;
        }
    }
}
