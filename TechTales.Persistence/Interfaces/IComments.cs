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
    public interface IComments : IBaseRepository<Comments>
    {

        Task<List<CommentsModel>> GetCommentsByUserId(int usuarioId);

        Task<List<CommentsModel>> GetCommentsByStoriesId(int historiaId);

        Task<int> GetCountCommentsByStoriesId(int historiaId);

    }
}
