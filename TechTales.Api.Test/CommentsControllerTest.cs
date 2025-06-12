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
using TechTales.Application.Dtos.ComentarioDto;

namespace TechTales.Api.Test
{
    public class CommentsControllerTest
    {
        private readonly Mock<ICommentsServices> _mockCommentService;
        private readonly CommentsController _controller;

        public CommentsControllerTest()
        {
            _mockCommentService = new Mock<ICommentsServices>();
            _controller = new CommentsController(_mockCommentService.Object);
        }

        [Fact]
        public async Task GetCommentsByUser_ReturnsOk_WhenSuccess()
        {
            // Arrange
            int usuarioId = 1;
            var serviceResult = new ServiceResult { Success = true, Data = "Comentarios de prueba" };
            _mockCommentService.Setup(s => s.GetCommentByUserId(usuarioId)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetCommentsByUserId(usuarioId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task GetCommentsByUser_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            int usuarioId = 1;
            var serviceResult = new ServiceResult { Success = false, Message = "Error al obtener comentarios" };
            _mockCommentService.Setup(s => s.GetCommentByUserId(usuarioId)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetCommentsByUserId(usuarioId);

            // Assert
            Assert.Equal(serviceResult.Message, "Error al obtener comentarios");
        }


        [Fact]
        public async Task GetCommentsByStoriesId_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            int storyId = 1;
            var serviceResult = new ServiceResult { Success = false, Message = "Error al obtener comentarios" };
            _mockCommentService.Setup(s => s.GetCommentByStoriesId(storyId)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetCommentsByStoriesId(storyId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult.Message, "Error al obtener comentarios");
        }

        [Fact]
        public async Task GetCountCommentByStoriesId_ReturnsOk_WhenSuccess()
        {
            // Arrange
            int storyId = 1;
            var serviceResult = new ServiceResult { Success = true, Data = 10 };  // Suponiendo que se devuelven los comentarios como número
            _mockCommentService.Setup(s => s.GetCountCommentByStoriesId(storyId)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetCountCommentByStoriesId(storyId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task GetCountCommentByStoriesId_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            int storyId = 1;
            var serviceResult = new ServiceResult { Success = false, Message = "Error al obtener el número de comentarios" };
            _mockCommentService.Setup(s => s.GetCountCommentByStoriesId(storyId)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetCountCommentByStoriesId(storyId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult.Message, "Error al obtener el número de comentarios");
        }










        [Fact]
        public async Task SaveComment_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var comentarioAddDto = new CommentsAddDto();
            var serviceResult = new ServiceResult { Success = true, Message = "Comentario guardado" };
            _mockCommentService.Setup(s => s.Add(comentarioAddDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Post(comentarioAddDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task SaveComment_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var comentarioAddDto = new CommentsAddDto();
            var serviceResult = new ServiceResult { Success = false, Message = "Error al guardar comentario" };
            _mockCommentService.Setup(s => s.Add(comentarioAddDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Post(comentarioAddDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteComment_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var comentarioRemoveDto = new CommentsRemoveDto();
            var serviceResult = new ServiceResult { Success = false, Message = "Error al eliminar comentario" };
            _mockCommentService.Setup(s => s.Remove(comentarioRemoveDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Delete(comentarioRemoveDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }
    }
}
