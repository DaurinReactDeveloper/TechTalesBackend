using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Dtos.VotosHistoriasDto;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Models;

namespace TechTales.Application.Validations
{
    public static class VotosHistoriaValidations
    {

        public static bool VotesStoriesAddValidation(VotesStoriesAddDto votesStoriesAdd, out string message)
        {
            message = string.Empty;

            if (votesStoriesAdd == null)
            {
                message = "El voto es nulo."; 
                return false; 
            }

            if (votesStoriesAdd.IdUser < 0)
            {
                message = "El ID del usuario es inválido."; 
                return false; 
            }

            if (votesStoriesAdd.IdStories < 0)
            {
                message = "El ID de la historia es inválido."; 
                return false; 
            }

            if (votesStoriesAdd.DateVote?.Date != DateTime.UtcNow.Date)
            {
                message = "La fecha del voto debe ser la fecha actual.";
                return false;
            }

            return true;
        }

        public static bool VotesStoriesUpdateValidation(VotesStoriesUpdateDto votesStoriesUpdate, out string message)
        {
            message = string.Empty;

            if(votesStoriesUpdate.Id <= 0)
            {
                message = "No se pudo obtener el voto";
                return false;

            }

            return true;
        }

        public static bool VotesStoriesRemoveValidation(VotesStoriesRemoveDto votesStoriesRemove, out string message)
        {
            message = string.Empty;

            if (votesStoriesRemove.Id <= 0)
            {
                message = "El ID del voto es inválido.";
                return false;
            }

            return true;
        }

        public static bool VotesStoriesVerifyId(VotesStories voteId, out string message)
        {
            message = string.Empty;

            if (voteId is null)
            {
                message = "No se pudo obtener el Voto.";
                return false;
            }

            return true;

        }

    }
}



