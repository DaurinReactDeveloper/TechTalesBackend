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
    public interface IChallenges : IBaseRepository<Challenges>
    {

        Task<List<ChallengesModel>> GetChallenges();

        Task<List<ChallengesModel>> GetChallengesForType(string type);
    }
}
