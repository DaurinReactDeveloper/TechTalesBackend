using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Core;
using TechTales.Application.Dtos.UsuarioDto;
using TechTales.Domain.entities;

namespace TechTales.Application.Contract
{
    public interface IUsersServices : IBaseServices<UsersAddDto, UsersRemoveDto>, IUpdateServices<UsersUpdateDto>
    {

        Task<ServiceResult> GetUsers();

        Task<ServiceResult> GetUser(string email, string password, string rol);

        Task<ServiceResult> GetUserById(int id);

        Task<ServiceResult> LoginWithGoogle(string email, string name);

        Task<ServiceResult> RegisterWithGoogle(UsersAddDto modelDto);

        Task<ServiceResult> AddAdminUser(UsersAddDto modelDto);

    }
}
