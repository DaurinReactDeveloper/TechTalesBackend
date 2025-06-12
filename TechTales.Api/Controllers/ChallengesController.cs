using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechTales.Application.Contract;
using TechTales.Application.Dtos.RetoDto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechTales.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengesController : ControllerBase
    {

        private readonly IChallengesServices _challengesServices;

        public ChallengesController(IChallengesServices challengesServices)
        {

            this._challengesServices = challengesServices;

        }

        // GET: api/<ChallengesController>
        [HttpGet("GetChallengesForType/{type}")]
        public async Task<IActionResult> GetChallengesForType(string type)
        {
            var result = await _challengesServices.GetChallengesForType(type);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);

        }

        // GET: api/<ChallengesController>
        [HttpGet("GetChallenges")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetChallenges()
        {
            var result = await _challengesServices.GetChallenges();

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);

        }

        // POST api/<ChallengesController>
        [HttpPost("Save")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Post([FromBody] ChallengesAddDto retoAdd)
        {

            var result = await _challengesServices.Add(retoAdd);

            if (!result.Success)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }

        // PUT api/<ChallengesController>/5
        [HttpPut("Update")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Put([FromBody] ChallengesUpdateDto retoUpdate)
        {

            var result = await _challengesServices.Update(retoUpdate);

            if (!result.Success)
            {
               return BadRequest(result);
            }

            return Ok(result);

        }

        // DELETE api/<ChallengesController>/5
        [HttpDelete("Delete")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromBody] ChallengesRemoveDto retoRemove)
        {

            var result = await _challengesServices.Remove(retoRemove);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);  

        }
    
    }
}
