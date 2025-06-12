using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTales.Application.Core;
using TechTales.Application.Dtos.ComentarioDto;
using TechTales.Application.Services;
using TechTales.Domain.entities;
using TechTales.Persistence.Interfaces;
using Microsoft.Extensions.Logging;
using Xunit;
using TechTales.Infrastructure.Models;

namespace TechTales.Application.Test
{
    public class CommentsServicesTest
    {
        private readonly Mock<IComments> _commentsMock;
        private readonly Mock<ILogger<CommentsServices>> _loggerMock;
        private readonly CommentsServices _commentsServices;

        public CommentsServicesTest()
        {
            _commentsMock = new Mock<IComments>();
            _loggerMock = new Mock<ILogger<CommentsServices>>();
            _commentsServices = new CommentsServices(_commentsMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetCommentsByUserId_ShouldReturnSuccess_WhenComentariosAreFound()
        {
            // Arrange
            var userId = 1;
            var comments = new List<CommentsModel>
            {
                new CommentsModel { Id = 1, Content = "Comentario 1", IdUser = userId }
            };

            _commentsMock.Setup(x => x.GetCommentsByUserId(userId)).ReturnsAsync(comments);

            // Act
            var result = await _commentsServices.GetCommentByUserId(userId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Comentarios del usuario obtenidos correctamente", result.Message);
        }

        [Fact]
        public async Task GetCommentsByUserId_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            var userId = 1;
            _commentsMock.Setup(x => x.GetCommentsByUserId(userId)).ThrowsAsync(new Exception("Error"));

            // Act
            var result = await _commentsServices.GetCommentByUserId(userId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Error obteniendo los comentarios del usuario.", result.Message);
        }


        [Fact]
        public async Task GetCommentByStoriesId_ShouldReturnSuccess_WhenCommentsAreFound()
        {
            // Arrange
            var storiesId = 1;
            var comments = new List<CommentsModel>
    {
        new CommentsModel { Id = 1, Content = "Comentario 1", IdStorie = storiesId }
    };

            _commentsMock.Setup(x => x.GetCommentsByStoriesId(storiesId)).ReturnsAsync(comments);

            // Act
            var result = await _commentsServices.GetCommentByStoriesId(storiesId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Comentarios de la historia obtenidos correctamente", result.Message);
        }

        [Fact]
        public async Task GetCommentByStoriesId_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            var storiesId = 1;
            _commentsMock.Setup(x => x.GetCommentsByStoriesId(storiesId)).ThrowsAsync(new Exception("Error"));

            // Act
            var result = await _commentsServices.GetCommentByStoriesId(storiesId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Error obteniendo los comentarios de la historia.", result.Message);
        }


        [Fact]
        public async Task GetCountCommentByStoriesId_ShouldReturnSuccess_WhenCountIsFetched()
        {
            // Arrange
            var storiesId = 1;
            var count = 5;

            _commentsMock.Setup(x => x.GetCountCommentsByStoriesId(storiesId)).ReturnsAsync(count);

            // Act
            var result = await _commentsServices.GetCountCommentByStoriesId(storiesId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Total de Comentarios de la historia obtenidos correctamente", result.Message);
            Assert.Equal(count, result.Data);
        }

        [Fact]
        public async Task GetCountCommentByStoriesId_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            var storiesId = 1;
            _commentsMock.Setup(x => x.GetCountCommentsByStoriesId(storiesId)).ThrowsAsync(new Exception("Error"));

            // Act
            var result = await _commentsServices.GetCountCommentByStoriesId(storiesId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Error obteniendo el total de comentarios de la historia.", result.Message);
        }

        [Fact]
        public async Task Add_ShouldReturnSuccess_WhenComentarioIsValid()
        {
            // Arrange
            var modelDto = new CommentsAddDto
            {
                Content = "Este es un comentario válido que cumple con el rango de longitud permitido, con un mínimo de 25 y un máximo de 400 caracteres.",
                IdUser = 1,
                DateComment = DateTime.Now,
                ChangeUser = 1
            };

            _commentsMock.Setup(x => x.Add(It.IsAny<Comments>())).Returns(Task.CompletedTask);
            _commentsMock.Setup(x => x.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _commentsServices.Add(modelDto);

            // Assert
            //Assert.True(result.Success);
            Assert.Equal("Comentario agregado correctamente", result.Message);
        }

        [Fact]
        public async Task Remove_ShouldReturnSuccess_WhenComentarioIsRemoved()
        {
            // Arrange
            var modelDto = new CommentsRemoveDto { Id = 1, ChangeUser = 1 };
            var comment = new Comments { Id = 1, Content = "Comentario para eliminar", IdUser = 1 };

            _commentsMock.Setup(x => x.GetById(1)).ReturnsAsync(comment);
            _commentsMock.Setup(x => x.Remove(It.IsAny<Comments>())).Returns(Task.CompletedTask);
            _commentsMock.Setup(x => x.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _commentsServices.Remove(modelDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Comentario eliminado correctamente.", result.Message);
        }

    }
}