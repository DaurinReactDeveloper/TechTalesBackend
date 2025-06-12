using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Models;

namespace TechTales.Infrastructure.Extensions
{
    public static class VotesStoriesExceptions
    {

        public static VotesStoriesModel ConvertVotosHistoriasEntityToModel(this VotesStories votoshistoriaEntity )
        {

            var votosModel = new VotesStoriesModel()
            {

                Id = votoshistoriaEntity.Id,
                IdUser = votoshistoriaEntity.IdUser,
                IdStories = votoshistoriaEntity.IdStories,
                Vote = votoshistoriaEntity.Vote,
                DateVote = votoshistoriaEntity.DateVote,

            };

            return votosModel;
        }


    }
}
