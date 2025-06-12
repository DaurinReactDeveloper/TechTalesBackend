using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Dtos.HistoriaDto;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Models;

namespace TechTales.Application.Validations
{
    public static class StoriesValidations
    {

        public static bool StoriesAddValidation(StoriesAddDto storiesAdd, out string message)
        {

            message = string.Empty;

            if (storiesAdd.Title.Length <= 15 || storiesAdd.Title.Length >= 100)
            {
                message = "El título debe tener una longitud entre 15 y 100 caracteres.";
                return false;
            }

            if (storiesAdd.Content.Length <= 100 || storiesAdd.Content.Length >= 400)
            {
                message = "El contenido debe tener una longitud entre 100 y 400 caracteres.";
                return false;
            }

            if (storiesAdd.IdUser <= 0)
            {
                message = "El ID del usuario debe ser un valor mayor a 0.";
                return false;
            }

            if (storiesAdd.DatePublication > DateTime.UtcNow)
            {
                message = "La fecha de publicación no puede ser en el futuro.";
                return false;
            }

            return true;
        }

        public static bool StoriesUpdateValidation(StoriesUpdateDto storiesUpdate, out string message)
        {

            message = string.Empty;

            if (storiesUpdate.Content.Length <= 100 || storiesUpdate.Content.Length >= 400)
            {
                message = "El contenido debe tener una longitud entre 100 y 400 caracteres.";
                return false;
            }

            return true;
        }

        public static bool StoriesRemoveValidation(StoriesRemoveDto storiesRemove, out string message)
        {

            message = string.Empty;

            if (storiesRemove.Id <= 0)
            {
                message = "No tiene los permisos necesarios para eliminar esta historia.";
                return false;
            }

            return true;
        }

        public static bool StoriesPermits(StoriesDto storiesRemove,Stories stories, out string message)
        {

            message = string.Empty;

            if (storiesRemove.IdUser != stories.IdUser)
            {
                message = "No tiene los permisos necesarios para eliminar esta historia.";
                return false;
            }

            return true;
        }

        public static bool StoriesVerifyId(Stories storieId,out string message)
        {
            message = string.Empty;

            if (storieId is null)
            {
                message = "No se pudo obtener la historia";
                return false;
            }

            return true;

        }

        public static bool StoriesCount(List<StoriesModel> storie,out string message)
        {

            message = string.Empty;

            if (storie.Count <= 0)
            {
                message = "No se encontraron historias disponibles.";
                return false;

            }

            return true;

        }

    }
}
