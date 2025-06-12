using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechTales.Application.Contract;
using TechTales.Application.Core;
using TechTales.Application.Dtos.ComentarioDto;
using TechTales.Application.Validations;
using TechTales.Domain.entities;
using TechTales.Persistence.Interfaces;

namespace TechTales.Application.Services
{
    public class CommentsServices : ICommentsServices
    {
        private readonly IComments _comment;
        private readonly ILogger<CommentsServices> _logger;

        public CommentsServices(IComments comentario, ILogger<CommentsServices> logger)
        {
            _comment = comentario;
            _logger = logger;
        }

        public async Task<ServiceResult> GetCommentByUserId(int userId)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var comments = await _comment.GetCommentsByUserId(userId);

                if (!CommentsValidations.CommentCount(comments, out string messageCount))
                {
                    result.Message = messageCount;
                    return result;
                }

                result.Data = comments;
                result.Message = "Comentarios del usuario obtenidos correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los comentarios del usuario.";
                _logger.LogError($"Ha ocurrido un error obteniendo los comentarios del usuario: {ex.Message}.");
            }

            return result;
        }

        public async Task<ServiceResult> GetCommentByStoriesId(int storiesId)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var comments = await _comment.GetCommentsByStoriesId(storiesId);

                if (!CommentsValidations.CommentCount(comments, out string messageCount))
                {
                    result.Message = messageCount;
                    return result;
                }

                result.Data = comments;
                result.Message = "Comentarios de la historia obtenidos correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los comentarios de la historia.";
                _logger.LogError($"Ha ocurrido un error obteniendo los comentarios de la historia: {ex.Message}.");
            }

            return result;
        }

        public async Task<ServiceResult> GetCountCommentByStoriesId(int storiesId)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var comments = await _comment.GetCountCommentsByStoriesId(storiesId);

                result.Data = comments;
                result.Message = "Total de Comentarios de la historia obtenidos correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo el total de comentarios de la historia.";
                _logger.LogError($"Ha ocurrido un error obteniendo el total de comentarios de la historia: {ex.Message}.");
            }

            return result;
        }

        public async Task<ServiceResult> Add(CommentsAddDto modelDto)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                if (!CommentsValidations.CommentsAddValidation(modelDto, out string message))
                {
                    result.Success = false;
                    result.Message = message;
                    return result;
                }

                var commentAdd = new Domain.entities.Comments
                {
                    Id = modelDto.Id,
                    Content = modelDto.Content,
                    IdStorie = modelDto.IdStorie,
                    IdUser = modelDto.IdUser,
                    DateComment = modelDto.DateComment,
                    CreationDate = DateTime.Now,
                    CreationUser = modelDto.ChangeUser
                };

                await _comment.Add(commentAdd);
                await _comment.SaveChanges();
                result.Message = "Comentario agregado correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el comentario.";
                _logger.LogError($"Ha ocurrido un error guardando el comentario: {ex.Message}.");
            }

            return result;
        }

        public async Task<ServiceResult> Remove(CommentsRemoveDto modelDto)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var commentRemove = await _comment.GetById(modelDto.Id);

                if (!CommentsValidations.CommentVerifyId(commentRemove, out string messageVerify))
                {
                    result.Success = false;
                    result.Message = messageVerify;
                    return result;
                }

                if (!CommentsValidations.CommentsRemoveValidation(modelDto, out string messageValidation))
                {
                    result.Success = false;
                    result.Message = messageValidation;
                    return result;
                }

                commentRemove.UserDeleted = modelDto.ChangeUser;

                await _comment.Remove(commentRemove);
                await _comment.SaveChanges();
                result.Message = "Comentario eliminado correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el comentario.";
                _logger.LogError($"Ha ocurrido un error eliminando el comentario: {ex.Message}.");
            }

            return result;
        }

    }
}