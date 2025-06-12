using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Contract;
using TechTales.Application.Core;
using TechTales.Application.Dtos.VotosHistoriasDto;
using TechTales.Application.Validations;
using TechTales.Domain.entities;
using TechTales.Persistence.Context;
using TechTales.Persistence.Interfaces;

namespace TechTales.Application.Services
{
    public class VotesStoriesServices : IVotesStoriesServices
    {

        private readonly IVotesStories _votesStories;
        private readonly ILogger<VotesStoriesServices> _logger;

        public VotesStoriesServices(IVotesStories votesStories, ILogger<VotesStoriesServices> logger)
        {
            this._votesStories = votesStories;
            this._logger = logger;
        }

        public async Task<ServiceResult> GetVotesByStoriesId(int storieId)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var (likes, dislikes) = await _votesStories.GetVotesByStoriesId(storieId);


                if (likes == 0 && dislikes == 0)
                {
                    result.Message = "No hay votos.";
                    return result;
                }

                result.Data = new { likes, dislikes };
                result.Message = "Votos obtenidos correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los votos.";
                _logger.LogError($"Ha ocurrido un error obteniendo los votos: {ex.Message}");
            }

            return result;
        }

        public async Task<ServiceResult> Add(VotesStoriesAddDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                if (!VotosHistoriaValidations.VotesStoriesAddValidation(modelDto, out string validationMessage))
                {
                    result.Success = false;
                    result.Message = validationMessage;
                    return result;
                }

                var existingVote = await _votesStories.GetVotesByUserAndStoriesTrueDelete(modelDto.IdUser, modelDto.IdStories);

                if (existingVote != null)
                {

                    if (existingVote.Deleted)
                    {
                        return await this.Update(BuildUpdateDto(existingVote, modelDto));
                    }

                    if (existingVote.Vote == modelDto.Vote)
                    {
                        return await this.Remove(BuildRemoveDto(existingVote, modelDto));
                    }
                    else
                    {
                        return await this.Update(BuildUpdateDto(existingVote, modelDto));
                    }
                }

                var newVote = new VotesStories
                {
                    IdUser = modelDto.IdUser,
                    IdStories = modelDto.IdStories,
                    Vote = modelDto.Vote,
                    DateVote = DateTime.Now,
                    CreationDate = DateTime.Now,
                    CreationUser = modelDto.ChangeUser
                };

                await _votesStories.Add(newVote);
                await _votesStories.SaveChanges();

                result.Message = "Voto agregado correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el voto.";
                _logger.LogError($"Ha ocurrido un error guardando el voto: {ex.Message}");
            }

            return result;
        }

        public async Task<ServiceResult> Update(VotesStoriesUpdateDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var votoExistente = await _votesStories.GetById(modelDto.Id);

                if (!VotosHistoriaValidations.VotesStoriesVerifyId(votoExistente, out string messageVerify))
                {
                    result.Success = false;
                    result.Message = messageVerify;
                    return result;
                }

                if (!VotosHistoriaValidations.VotesStoriesUpdateValidation(modelDto, out string messageValidation))
                {
                    result.Success = false;
                    result.Message = messageValidation;
                    return result;
                }

                votoExistente.Vote = modelDto.Vote;
                votoExistente.ModifyDate = DateTime.Now;
                votoExistente.UserMod = modelDto.ChangeUser;

                await _votesStories.Update(votoExistente);
                await _votesStories.SaveChanges();

                result.Message = "Voto actualizado correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el voto.";
                _logger.LogError($"Error actualizando voto: {ex.Message}");
            }

            return result;
        }

        public async Task<ServiceResult> Remove(VotesStoriesRemoveDto modelDto)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var voteStorieRemove = await _votesStories.GetById(modelDto.Id);

                if (!VotosHistoriaValidations.VotesStoriesVerifyId(voteStorieRemove, out string messageVerify))
                {
                    result.Success = false;
                    result.Message = messageVerify;
                    return result;
                }

                if (!VotosHistoriaValidations.VotesStoriesRemoveValidation(modelDto, out string messageValidation))
                {
                    result.Success = false;
                    result.Message = messageValidation;
                    return result;
                }

                voteStorieRemove.UserDeleted = modelDto.ChangeUser;

                await _votesStories.Remove(voteStorieRemove);
                await _votesStories.SaveChanges();
                result.Message = "Voto eliminado correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el voto de la historia.";
                _logger.LogError($"Ha ocurrido un error eliminando el voto de la historia: {ex.Message}.");
            }

            return result;
        }

        public VotesStoriesUpdateDto BuildUpdateDto(VotesStories existingVote, VotesStoriesAddDto modelDto)
        {
            return new VotesStoriesUpdateDto
            {
                Id = existingVote.Id,
                Vote = modelDto.Vote,
                ChangeUser = modelDto.ChangeUser
            };
        }

        public VotesStoriesRemoveDto BuildRemoveDto(VotesStories existingVote, VotesStoriesAddDto modelDto)
        {
            return new VotesStoriesRemoveDto
            {
                Id = existingVote.Id,
                ChangeUser = modelDto.ChangeUser
            };
        }

    }
}
