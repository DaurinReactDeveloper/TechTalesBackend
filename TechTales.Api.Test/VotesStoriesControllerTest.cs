using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Api.Controllers;
using TechTales.Application.Contract;
using TechTales.Application.Core;
using TechTales.Application.Dtos.VotosHistoriasDto;

namespace TechTales.Api.Test
{
    public class VotesStoriesControllerTest
    {


        private readonly Mock<IVotesStoriesServices> _mockVotesServices;
        private readonly VotesStoriesController _controller;

        public VotesStoriesControllerTest()
        {
            _mockVotesServices = new Mock<IVotesStoriesServices>();
            _controller = new VotesStoriesController(_mockVotesServices.Object);
        }

        [Fact]
        public async Task GetVotesByStories_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var id = 1;
            var serviceResult = new ServiceResult { Success = true, Data = "Votos encontrados" };
            _mockVotesServices.Setup(s => s.GetVotesByStoriesId(id)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetVotesByStoriesId(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task GetVotesByStories_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var id = 1;
            var serviceResult = new ServiceResult { Success = false, Message = "Error al obtener votos" };
            _mockVotesServices.Setup(s => s.GetVotesByStoriesId(id)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetVotesByStoriesId(id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }

        [Fact]
        public async Task Post_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var votosHistoriaAddDto = new VotesStoriesAddDto { IdStories = 1, Vote = true };
            var serviceResult = new ServiceResult { Success = true, Message = "Voto guardado" };
            _mockVotesServices.Setup(s => s.Add(votosHistoriaAddDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Post(votosHistoriaAddDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var votosHistoriaAddDto = new VotesStoriesAddDto { IdStories = 1, Vote = true };
            var serviceResult = new ServiceResult { Success = false, Message = "Error al guardar el voto" };
            _mockVotesServices.Setup(s => s.Add(votosHistoriaAddDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Post(votosHistoriaAddDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }
    }
}