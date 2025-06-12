using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Domain.entities;
using TechTales.Domain.Repository;
using TechTales.Infrastructure.Models;

namespace TechTales.Persistence.Interfaces
{
    public interface IVotesStories : IBaseRepository<VotesStories>
    {

        public Task<(int likes, int dislikes)> GetVotesByStoriesId(int idHistoria);

        Task<VotesStories> GetVotesByUserAndStories(int idUsuario, int idHistoria);

        Task<VotesStories> GetVotesByUserAndStoriesTrueDelete(int userId, int storyId);


    }
}
