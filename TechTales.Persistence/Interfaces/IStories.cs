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
    public interface IStories : IBaseRepository<Stories>
    {

        Task<List<StoriesModel>> GetStories();

        Task<List<StoriesModel>> GetStoriesByUserId(int usuarioId);   

     }
}
