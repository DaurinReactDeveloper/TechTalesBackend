using System;
using System.Collections.Generic;
using TechTales.Domain.entities;
using TechTales.Domain.Repository;
using TechTales.Infrastructure.Models;

namespace TechTales.Persistence.Interfaces
{
    public interface IUsers : IBaseRepository<Users>
    {

        Task<List<UsersModel>> GetUsers();

        Task<UsersModel> GetUserLogin(string email, string password, string rol);

        Task<UsersModel> GetUserEmail(string email);

        Task<UsersModel> GetUserById(int id);


    }
}
