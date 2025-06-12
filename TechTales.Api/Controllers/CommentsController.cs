using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechTales.Application.Contract;
using TechTales.Application.Dtos.ComentarioDto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechTales.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {

        private readonly ICommentsServices _commentsServices;

        public CommentsController(ICommentsServices commentsServices)
        {

            this._commentsServices = commentsServices;

        }

        // GET api/<CommentsController>/5
        [HttpGet("GetCommentsByUserId/{id}")]
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> GetCommentsByUserId(int id)
        {
            var result = await _commentsServices.GetCommentByUserId(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        // GET api/<CommentsController>/5
        [HttpGet("GetCommentsByStoriesId/{id}")]
        public async Task<IActionResult> GetCommentsByStoriesId(int id)
        {
            var result = await _commentsServices.GetCommentByStoriesId(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        [HttpGet("GetCountCommentByStoriesId/{id}")]
        public async Task<IActionResult> GetCountCommentByStoriesId(int id)
        {
            var result = await _commentsServices.GetCountCommentByStoriesId(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        // POST api/<CommentsController>
        [HttpPost("Save")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Post([FromBody] CommentsAddDto comentarioAdd)
        {

            var result = await _commentsServices.Add(comentarioAdd);

            if (!result.Success)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }

        // DELETE api/<CommentsController>/5
        [HttpDelete("Delete")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Delete([FromBody] CommentsRemoveDto comentarioRemove)
        {

            var result = await _commentsServices.Remove(comentarioRemove);

            if (!result.Success)
            {

                return BadRequest(result);

            }

            return Ok(result);

        }
    
    }
}
