using System;
using System.Collections.Generic;
using TechTales.Domain.entities;
using TechTales.Domain.Repository;
using TechTales.Infrastructure.Models;

namespace TechTales.Persistence.Interfaces
{
    public interface IChallengesCompleted : IBaseRepository<ChallengesCompleted>
    {

        Task<List<ChallengesCompletedModel>> GetChallengesCompletedByUserId(int usuarioId);

        Task<bool> VerifyChallengesCompleted(int idChallenges, int idUser);

        Task<ChallengesCompleted?> GetDeletedChallenge(int idChallenges, int idUser);
    }
}
