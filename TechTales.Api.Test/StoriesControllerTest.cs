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
using TechTales.Application.Dtos.HistoriaDto;

namespace TechTales.Api.Test
{
    public class StoriesControllerTest
    {
        private readonly Mock<IStoriesServices> _mockStoriesService;
        private readonly StoriesController _controller;

        public StoriesControllerTest()
        {
            _mockStoriesService = new Mock<IStoriesServices>();
            _controller = new StoriesController(_mockStoriesService.Object);
        }

        [Fact]
        public async Task GetStories_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var serviceResult = new ServiceResult { Success = true, Data = "Lista de historias" };
            _mockStoriesService.Setup(s => s.GetStories()).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetStories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task GetStories_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var serviceResult = new ServiceResult { Success = false, Message = "Error al obtener historias" };
            _mockStoriesService.Setup(s => s.GetStories()).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetStories();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error al obtener historias", badRequestResult.Value);
        }

        [Fact]
        public async Task GetStoriesByUser_ReturnsOk_WhenSuccess()
        {
            // Arrange
            int usuarioId = 1;
            var serviceResult = new ServiceResult { Success = true, Data = "Historias del usuario" };
            _mockStoriesService.Setup(s => s.GetStoriesByUserId(usuarioId)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetStoriesByUserId(usuarioId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task GetStoriesByUser_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            int usuarioId = 1;
            var serviceResult = new ServiceResult { Success = false, Message = "Error al obtener historias del usuario" };
            _mockStoriesService.Setup(s => s.GetStoriesByUserId(usuarioId)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetStoriesByUserId(usuarioId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }

        [Fact]
        public async Task SaveStorie_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var historiaAddDto = new StoriesAddDto();
            var serviceResult = new ServiceResult { Success = true, Message = "Historia guardada" };
            _mockStoriesService.Setup(s => s.Add(historiaAddDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Post(historiaAddDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task SaveStorie_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var historiaAddDto = new StoriesAddDto();
            var serviceResult = new ServiceResult { Success = false, Message = "Error al guardar historia" };
            _mockStoriesService.Setup(s => s.Add(historiaAddDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Post(historiaAddDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateStorie_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var historiaUpdateDto = new StoriesUpdateDto();
            var serviceResult = new ServiceResult { Success = true, Message = "Historia actualizada" };
            _mockStoriesService.Setup(s => s.Update(historiaUpdateDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Put(historiaUpdateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task UpdateStorie_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var historiaUpdateDto = new StoriesUpdateDto();
            var serviceResult = new ServiceResult { Success = false, Message = "Error al actualizar historia" };
            _mockStoriesService.Setup(s => s.Update(historiaUpdateDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Put(historiaUpdateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteStorie_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var historiaRemoveDto = new StoriesRemoveDto();
            var serviceResult = new ServiceResult { Success = true, Message = "Historia eliminada" };
            _mockStoriesService.Setup(s => s.Remove(historiaRemoveDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Delete(historiaRemoveDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task DeleteStorie_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var historiaRemoveDto = new StoriesRemoveDto();
            var serviceResult = new ServiceResult { Success = false, Message = "Error al eliminar historia" };
            _mockStoriesService.Setup(s => s.Remove(historiaRemoveDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Delete(historiaRemoveDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }

    }
}
