using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Dtos.RetosCompletados;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Models;

namespace TechTales.Application.Validations
{
    public static class ChallengesCompletedValidations
    {

        public static bool ChallengesCompletadosAddValidation(ChallengesCompletedAddDto challengesCompletedAdd, out string message)
        {
            message = string.Empty;

            if (challengesCompletedAdd.IdUser <= 0)
            {
                message = "El ID del usuario debe ser un valor mayor a 0.";
                return false;
            }

            if (challengesCompletedAdd.IdChallenges <= 0)
            {
                message = "El ID del reto debe ser un valor mayor a 0.";
                return false;
            }

            return true;
        }

        public static bool ChallengesCompletadosRemoveValidation(ChallengesCompletedRemoveDto challengesCompletedRemove, out string message)
        {
            message = string.Empty;

            if (challengesCompletedRemove.Id <= 0)
            {
                message = "El ID del reto debe ser mayor a 0. No se pudo obtener el reto.";
                return false;
            }

            return true;
        }

        public static bool ChallengesCompletadosVerifyId(ChallengesCompleted challengeCompletedId, out string message)
        {
            message = string.Empty;

            if (challengeCompletedId is null)
            {
                message = "No se pudo obtener el reto completado.";
                return false;
            }

            return true;

        }

        public static bool ChallengesCount(List<ChallengesCompletedModel> challengesCompleted, out string message)
        {

            message = string.Empty;

            if (challengesCompleted.Count <= 0)
            {
                message = "No tiene retos completados.";
                return false;

            }


            return true;

        }

        }

    }

