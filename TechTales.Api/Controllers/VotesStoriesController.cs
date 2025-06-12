using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechTales.Application.Contract;
using TechTales.Application.Dtos.VotosHistoriasDto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechTales.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotesStoriesController : ControllerBase
    {

        private readonly IVotesStoriesServices _votesStoriesServices;

        public VotesStoriesController(IVotesStoriesServices votesStoriesServices)
        {

            this._votesStoriesServices = votesStoriesServices;

        }

        // GET: api/<VotesStoriesController>
        [HttpGet("GetVotesByStoriesId/{id}")]
        public async Task<IActionResult> GetVotesByStoriesId(int id)
        {
            var result = await _votesStoriesServices.GetVotesByStoriesId(id);

            if(!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // POST api/<VotesStoriesController>
        [HttpPost("Save")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Post([FromBody] VotesStoriesAddDto votosHistoriaAdd)
        {

            var result = await _votesStoriesServices.Add(votosHistoriaAdd);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

    }
}
