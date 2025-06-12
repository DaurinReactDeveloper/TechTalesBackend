using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Models;

namespace TechTales.Infrastructure.Extensions
{
    public static class ChallengesExtension
    {

        public static ChallengesModel ConvertRetoEntityToModel(this Challenges retoEntity)
        {
            var retoModel = new ChallengesModel()
            {

                Id = retoEntity.Id,
                Title = retoEntity.Title,
                Description = retoEntity.Description,
                DatePublication = retoEntity.DatePublication,
                Type = retoEntity.Type,
                Hint = retoEntity.Hint,
                IdAdmin = retoEntity.IdAdmin,

            };

            return retoModel;
        }


    }
}
