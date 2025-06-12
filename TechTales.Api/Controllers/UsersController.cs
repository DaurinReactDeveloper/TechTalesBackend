using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechTales.Application.Contract;
using TechTales.Application.Dtos.UsuarioDto;
using TechTales.Application.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechTales.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUsersServices _userServices;
        private readonly IConfiguration _configuration;

        public UsersController(IUsersServices userServices, IConfiguration configuration)
        {

            this._userServices = userServices;
            this._configuration = configuration;

        }

        // GET: api/<UsersController>
        [HttpGet("GetUsers")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsers()
        {

            var result = await _userServices.GetUsers();

            if (!result.Success)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }

        // GET api/<UsersController>/5
        [HttpGet("GetUser/{email}/{password}")]
        public async Task<IActionResult> GetUser(string email, string password)
        {

            var result = await _userServices.GetUser(email, password, "user");

            if (!result.Success)
            {
                return BadRequest(result);
            }

            var token = JwtServices.GenerateToken(result.Data.Name, result.Data.Role, _configuration);

            return Ok(new
            {
                Data = result,
                Token = token
            });

        }

        // GET api/<UsersController>/5
        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {

            var result = await _userServices.GetUserById(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        // GET api/<UsersController>/5
        [HttpGet("GetUserAdmin/{email}/{password}")]
        public async Task<IActionResult> GetUserAdmin(string email, string password)
        {

            var result = await _userServices.GetUser(email, password, "admin");

            if (!result.Success)
            {
                return BadRequest(result);
            }

            var token = JwtServices.GenerateToken(result.Data.Name, result.Data.Role, _configuration);

            return Ok(new
            {
                Data = result,
                Token = token
            });

        }

        // GET api/<UsersController>/5
        [HttpGet("GetUserWithGoogle/{email}/{name}")]
        public async Task<IActionResult> GetUserWithGoogle(string email, string name)
        {

            var result = await _userServices.LoginWithGoogle(email, name);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            var token = JwtServices.GenerateToken(result.Data.Name, result.Data.Role, _configuration);

            return Ok(new
            {
                Data = result,
                Token = token
            });

        }

        // POST api/<UsersController>
        [HttpPost("RegisterWithGoogle")]
        public async Task<IActionResult> RegisterWithGoogle([FromBody] UsersAddDto usuarioAdd)
        {


            var result = await _userServices.RegisterWithGoogle(usuarioAdd);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            var token = JwtServices.GenerateToken(result.Data.Name, result.Data.Role, _configuration);

            return Ok(new
            {
                Data = result,
                Token = token
            });

        }

        // POST api/<UsersController>
        [HttpPost("Save")]
        public async Task<IActionResult> Post([FromBody] UsersAddDto usuarioAdd)
        {
            var result = await _userServices.Add(usuarioAdd);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        // POST api/<UsersController>
        [HttpPost("SaveAdmin")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PostAdmin([FromBody] UsersAddDto usuarioAdd)
        {
            var result = await _userServices.AddAdminUser(usuarioAdd);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }


        // PUT api/<UsersController>/5
        [HttpPut("Update")]
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> Put([FromBody] UsersUpdateDto usuarioUpdate)
        {

            var result = await _userServices.Update(usuarioUpdate);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        // DELETE api/<UsersController>/5
        [HttpDelete("Delete")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromBody] UsersRemoveDto usuarioRemove)
        {

            var result = await _userServices.Remove(usuarioRemove);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

    }
}

// Admin: Crear (usuario), Modificar (usuario), Eliminar (Usuario, product), Ver(Usuarios). 
// Seller: Crear (productos), Modificar (productos), Ver (productos).  - vendedor
// User: Ver (productos)