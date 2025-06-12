using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TechTales.Domain.Repository
{
    public interface IBaseRepository <Entity> where Entity : class
    {

        Task<List<Entity>> GetEntity();
        Task<Entity> GetById(int id);
        Task Add(Entity entity);
        Task Update(Entity entity);
        Task Remove(Entity entity);
        Task SaveChanges();

    }
}

