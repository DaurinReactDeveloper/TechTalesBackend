using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechTales.Application.Contract;
using TechTales.Application.Dtos.RetosCompletados;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechTales.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengesCompletedController : ControllerBase
    {

        private readonly IChallengesCompletedServices _challengesCompletedServices;

        public ChallengesCompletedController(IChallengesCompletedServices challengesCompletedServices)
        {

            this._challengesCompletedServices = challengesCompletedServices;

        }

        // GET: api/<ChallengesCompletedController>
        [HttpGet("GetChallengesCompletedByUser/{id}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> GetChallengesCompletedByUser(int id)
        {

            var result = await _challengesCompletedServices.GetChallengesCompletedByUserId(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        // POST api/<ChallengesCompletedController>
        [HttpPost("Save")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Post([FromBody] ChallengesCompletedAddDto completadosAdd)
        {

            var result = await _challengesCompletedServices.Add(completadosAdd);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        // DELETE api/<ChallengesCompletedController>/5
        [HttpDelete("Delete")]
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> Delete([FromBody] ChallengesCompletedRemoveDto retosCompletadosRemove)
        {

            var result = await _challengesCompletedServices.Remove(retosCompletadosRemove);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }
    
    }
}
