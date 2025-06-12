using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Dtos.RetoDto;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Models;

namespace TechTales.Application.Validations
{
    public static class ChallengesValidations
    {

        public static bool ChallengesAddValidation(ChallengesAddDto challengesAdd, out string message)
        {

            message = string.Empty;

            if (challengesAdd.Title.Length <= 15 || challengesAdd.Title.Length >= 100)
            {
                message = "El título debe tener entre 16 y 99 caracteres.";
                return false;
            }

            if (challengesAdd.Description.Length <= 100 || challengesAdd.Description.Length >= 400)
            {
                message = "La descripción debe tener entre 101 y 399 caracteres.";
                return false;
            }

            if (challengesAdd.Type != "basic" && challengesAdd.Type != "intermediate" && challengesAdd.Type != "advanced")
            {
                message = "El Tipo debe ser: basic, intermediate o advanced.";
                return false;
            }

            if (challengesAdd.IdAdmin <= 0)
            {
                message = "El Id del administrador debe ser un valor positivo.";
                return false;
            }

            if (challengesAdd.DatePublication > DateTime.UtcNow)
            {
                message = "La fecha de publicación no puede ser en el futuro.";
                return false;
            }

            if (challengesAdd.Hint.Length < 20 || challengesAdd.Hint.Length > 150)
            {
                message = "La pista debe tener entre 20 y 150 caracteres.";
                return false;
            }

            return true;
        }

        public static bool ChallengesUpdateValidation(ChallengesUpdateDto challengesUpdate, out string message)
        {

            message = string.Empty;

            if (challengesUpdate.Title.Length <= 15 || challengesUpdate.Title.Length >= 100)
            {
                message = "El título debe tener entre 16 y 99 caracteres.";
                return false;
            }

            if (challengesUpdate.Description.Length <= 100 || challengesUpdate.Description.Length >= 400)
            {
                message = "La descripción debe tener entre 101 y 399 caracteres.";
                return false;

            }

            if (challengesUpdate.Type != "basic" && challengesUpdate.Type != "intermediate" && challengesUpdate.Type != "advanced")
            {
                message = "El Tipo debe ser: basic, intermediate o advanced.";
                return false;
            }

            return true;

        }

        public static bool ChallengesRemoveValidation(ChallengesRemoveDto challengesRemove, out string message)
        {

            message = string.Empty;

            if (challengesRemove.Id <= 0)
            {
                message = "El Id del reto debe ser un valor positivo y mayor que 0.";
                return false;
            }

            return true;
        }

        public static bool ChallengesVerifyId(Challenges challengesId, out string message)
        {
            message = string.Empty;

            if (challengesId is null)
            {
                message = "No se pudo obtener el reto.";
                return false;
            }

            return true;

        }

        public static bool ChallengesCount(List<ChallengesModel> challenges, out string message)
        {

            message = string.Empty;

            if (challenges.Count <= 0)
            {
                message = "No se encontraron retos disponibles.";
                return false;

            }


            return true;

        }

    }
}
