using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Dtos.ComentarioDto;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Exceptions;
using TechTales.Infrastructure.Models;

namespace TechTales.Application.Validations
{
    public static class CommentsValidations
    {

        public static bool CommentsAddValidation(CommentsAddDto commentsAdd, out string message)
        {
            message = string.Empty;

            if (commentsAdd is null)
            {
                message = "El comentario no puede ser nulo.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(commentsAdd.Content))
            {
                message = "El contenido del comentario no puede estar vacío.";
                return false;
            }

            if (commentsAdd.Content.Length <= 15 || commentsAdd.Content.Length >= 400)
            {
                message = "El contenido debe tener entre 15 y 400 caracteres.";
                return false;
            }

            if (commentsAdd.IdUser <= 0)
            {
                message = "El ID del usuario no es válido.";
                return false;
            }


            if (commentsAdd.DateComment > DateTime.UtcNow)
            {
                message = "La fecha del comentario no puede ser en el futuro.";
                return false;
            }

            return true;
        }

        public static bool CommentsUpdateValidation(CommentsUpdateDto commentsUpdate, out string message)
        {
            message = string.Empty;

            if (commentsUpdate.Content.Length < 25 || commentsUpdate.Content.Length > 400)
            {
                message = "El contenido debe tener entre 25 y 400 caracteres.";
                return false;
            }

            return true;
        }

        public static bool CommentsRemoveValidation(CommentsRemoveDto commentsRemove, out string message)
        {
            message = string.Empty;

            if (commentsRemove.Id <= 0)
            {
                message = "El ID del comentario es inválido.";
                return false;
            }

            return true;
        }

        public static bool CommentVerifyId(Comments commentId, out string message)
        {
            message = string.Empty;

            if (commentId is null)
            {
                message = "No se pudo obtener el comentario";
                return false;
            }

            return true;

        }

        public static bool CommentCount(List<CommentsModel> comments, out string message)
        {

            message = string.Empty;

            if (comments.Count <= 0)
            {
                message = "No se encontraron los comentarios disponibles.";
                return false;

            }


            return true;

        }


    }

}
