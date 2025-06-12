using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechTales.Application.Contract;
using TechTales.Application.Dtos.HistoriaDto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechTales.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase
    {

        private readonly IStoriesServices _storiesServices;

        public StoriesController(IStoriesServices storiesServices)
        {

            this._storiesServices = storiesServices;

        }

        // GET: api/<StoriesController>
        [HttpGet("GetStories")]
        public async Task<IActionResult> GetStories()
        {

            var result = await _storiesServices.GetStories();

            if(!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);

        }

        // GET api/<StoriesController>/5
        [HttpGet("GetStoriesByUserId/{id}")]
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> GetStoriesByUserId(int id)
        {

            var result = await _storiesServices.GetStoriesByUserId(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        // POST api/<StoriesController>
        [HttpPost("Save")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Post([FromBody]StoriesAddDto historiaAdd)
        {
            var result = await _storiesServices.Add(historiaAdd);

            if (!result.Success)
            {

                return BadRequest(result);

            }

            return Ok(result);
        }

        // PUT api/<StoriesController>/5
        [HttpPut("Update")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Put([FromBody] StoriesUpdateDto historiaUpdate)
        {
            
            var result = await _storiesServices.Update(historiaUpdate);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        // DELETE api/<StoriesController>/5
        [HttpDelete("Delete")]
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> Delete([FromBody] StoriesRemoveDto removeDto)
        {

            var result = await _storiesServices.Remove(removeDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

    }
}
