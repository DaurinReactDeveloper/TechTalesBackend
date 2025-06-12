using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Models;

namespace TechTales.Infrastructure.Extensions
{
    public static class ChallengesCompletedExtension
    {

        public static ChallengesCompletedModel ConvertRetosCompletadoEntityToModel(this ChallengesCompleted retosCompletadoEntity)
        {

            var retoscompletadosModel = new ChallengesCompletedModel()
            {

                Id = retosCompletadoEntity.Id,
                IdUser = retosCompletadoEntity.IdUser,
                IdChallenges = retosCompletadoEntity.IdChallenges,
                DateCompleted = retosCompletadoEntity.DateCompleted,

            };

            return retoscompletadosModel;
        }


    }
}
