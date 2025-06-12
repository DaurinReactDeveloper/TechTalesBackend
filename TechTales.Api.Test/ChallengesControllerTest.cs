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
using TechTales.Application.Dtos.RetoDto;

namespace TechTales.Api.Test
{
    public class ChallengesControllerTest
    {

        private readonly Mock<IChallengesServices> _mockChallengesService;
        private readonly ChallengesController _controller;

        public ChallengesControllerTest()
        {
            _mockChallengesService = new Mock<IChallengesServices>();
            _controller = new ChallengesController(_mockChallengesService.Object);
        }

        [Fact]
        public async Task GetChallenges_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var serviceResult = new ServiceResult { Success = true, Message = "Challenges fetched", Data = new List<ChallengeResult>() };
            _mockChallengesService.Setup(s => s.GetChallenges()).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetChallenges();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task GetChallenges_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var serviceResult = new ServiceResult { Success = false, Message = "Error fetching challenges" };
            _mockChallengesService.Setup(s => s.GetChallenges()).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetChallenges();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult.Message, badRequestResult.Value);
        }

        [Fact]
        public async Task GetChallengesForType_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var type = "SomeType";
            var serviceResult = new ServiceResult { Success = true, Message = "Challenges fetched for type", Data = new List<ChallengeResult>() };
            _mockChallengesService.Setup(s => s.GetChallengesForType(type)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetChallengesForType(type);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task GetChallengesForType_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var type = "SomeType";
            var serviceResult = new ServiceResult { Success = false, Message = "Error fetching challenges for type" };
            _mockChallengesService.Setup(s => s.GetChallengesForType(type)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetChallengesForType(type);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult.Message, badRequestResult.Value);
        }


        [Fact]
        public async Task SaveChallenge_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var retoAddDto = new ChallengesAddDto();
            var serviceResult = new ServiceResult { Success = true, Message = "Reto guardado" };
            _mockChallengesService.Setup(s => s.Add(retoAddDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Post(retoAddDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task SaveChallenge_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var retoAddDto = new ChallengesAddDto();
            var serviceResult = new ServiceResult { Success = false, Message = "Error al guardar reto" };
            _mockChallengesService.Setup(s => s.Add(retoAddDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Post(retoAddDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateChallenge_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var retoUpdateDto = new ChallengesUpdateDto();
            var serviceResult = new ServiceResult { Success = true, Message = "Reto actualizado" };
            _mockChallengesService.Setup(s => s.Update(retoUpdateDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Put(retoUpdateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task UpdateChallenge_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var retoUpdateDto = new ChallengesUpdateDto();
            var serviceResult = new ServiceResult { Success = false, Message = "Error al actualizar reto" };
            _mockChallengesService.Setup(s => s.Update(retoUpdateDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Put(retoUpdateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteChallenge_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var retoRemoveDto = new ChallengesRemoveDto();
            var serviceResult = new ServiceResult { Success = true, Message = "Reto eliminado" };
            _mockChallengesService.Setup(s => s.Remove(retoRemoveDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Delete(retoRemoveDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task DeleteChallenge_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var retoRemoveDto = new ChallengesRemoveDto();
            var serviceResult = new ServiceResult { Success = false, Message = "Error al eliminar reto" };
            _mockChallengesService.Setup(s => s.Remove(retoRemoveDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Delete(retoRemoveDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }
    }
}
