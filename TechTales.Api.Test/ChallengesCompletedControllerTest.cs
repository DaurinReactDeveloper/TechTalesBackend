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
using TechTales.Application.Dtos.RetosCompletados;

namespace TechTales.Api.Test
{
    public class ChallengesCompletedControllerTest
    {

        private readonly Mock<IChallengesCompletedServices> _mockChallengesCompletedService;
        private readonly ChallengesCompletedController _controller;

        public ChallengesCompletedControllerTest()
        {
            _mockChallengesCompletedService = new Mock<IChallengesCompletedServices>();
            _controller = new ChallengesCompletedController(_mockChallengesCompletedService.Object);
        }

        [Fact]
        public async Task GetChallengesCompletedByUser_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var idUsuario = 1;
            var serviceResult = new ServiceResult { Success = true, Data = "Retos completados del usuario" };
            _mockChallengesCompletedService.Setup(s => s.GetChallengesCompletedByUserId(idUsuario)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetChallengesCompletedByUser(idUsuario);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task GetChallengesCompletedByUser_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var idUsuario = 1;
            var serviceResult = new ServiceResult { Success = false, Message = "Error al obtener los retos completados" };
            _mockChallengesCompletedService.Setup(s => s.GetChallengesCompletedByUserId(idUsuario)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetChallengesCompletedByUser(idUsuario);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var resultValue = Assert.IsType<ServiceResult>(badRequestResult.Value);  
            Assert.Equal("Error al obtener los retos completados", resultValue.Message);  
        }

        [Fact]
        public async Task SaveChallengesCompleted_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var retoCompletadoAddDto = new ChallengesCompletedAddDto();
            var serviceResult = new ServiceResult { Success = true, Message = "Reto completado guardado" };
            _mockChallengesCompletedService.Setup(s => s.Add(retoCompletadoAddDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Post(retoCompletadoAddDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task SaveChallengesCompleted_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var retoCompletadoAddDto = new ChallengesCompletedAddDto();
            var serviceResult = new ServiceResult { Success = false, Message = "Error al guardar el reto completado" };
            _mockChallengesCompletedService.Setup(s => s.Add(retoCompletadoAddDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Post(retoCompletadoAddDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteChallengesCompleted_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var retoCompletadoRemoveDto = new ChallengesCompletedRemoveDto();
            var serviceResult = new ServiceResult { Success = true, Message = "Reto completado eliminado" };
            _mockChallengesCompletedService.Setup(s => s.Remove(retoCompletadoRemoveDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Delete(retoCompletadoRemoveDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task DeleteChallengesCompleted_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var retoCompletadoRemoveDto = new ChallengesCompletedRemoveDto();
            var serviceResult = new ServiceResult { Success = false, Message = "Error al eliminar el reto completado" };
            _mockChallengesCompletedService.Setup(s => s.Remove(retoCompletadoRemoveDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Delete(retoCompletadoRemoveDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }

    }
}
