using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Models;
using TechTales.Persistence.Interfaces;
using TechTales.Persistence.Repositories;
using Xunit;

namespace TechTales.Persistence.Test
{
    public class CommentsRepositoryTest
    {

        private readonly Mock<IComments> _mockCommentsRepository;
        private readonly Mock<ILogger<CommentsRepository>> _mockLogger;

        public CommentsRepositoryTest()
        {
            _mockCommentsRepository = new Mock<IComments>();
            _mockLogger = new Mock<ILogger<CommentsRepository>>();

            _mockCommentsRepository.Setup(repo => repo.GetCommentsByUserId(It.IsAny<int>()))
                .ReturnsAsync((int usuarioId) =>
                {
                    if (usuarioId == 1)
                    {
                        return new List<CommentsModel>
                        {
                            new CommentsModel { Id = 1, Content = "Primer comentario", DateComment = DateTime.Now }
                        };
                    }
                    return new List<CommentsModel>
                    {
                        new CommentsModel { Id = 1, Content = "Primer comentario", DateComment = DateTime.Now },
                        new CommentsModel { Id = 2, Content = "Segundo comentario", DateComment = DateTime.Now }
                    };
                });

            // Configuración del mock para métodos CRUD
            _mockCommentsRepository.Setup(repo => repo.Add(It.IsAny<Comments>())).Returns(Task.CompletedTask);
            _mockCommentsRepository.Setup(repo => repo.Update(It.IsAny<Comments>())).Returns(Task.CompletedTask);
            _mockCommentsRepository.Setup(repo => repo.Remove(It.IsAny<Comments>())).Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task GetCommentsByUserId_ShouldReturnCommentForUserId()
        {
            // Arrange
            int usuarioId = 1;

            // Act
            var result = await _mockCommentsRepository.Object.GetCommentsByUserId(usuarioId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result); // Solo un comentario esperado
            Assert.Equal("Primer comentario", result[0].Content);
        }

        [Fact]
        public async Task GetCommentsByStoriesId_ShouldReturnCommentsForStory()
        {
            // Arrange
            var historiaId = 1;

            _mockCommentsRepository.Setup(repo => repo.GetCommentsByStoriesId(historiaId))
                .ReturnsAsync(new List<CommentsModel>
                {
            new CommentsModel { Id = 1, IdStorie = historiaId, Content = "Comentario historia 1", DateComment = DateTime.Now },
            new CommentsModel { Id = 2, IdStorie = historiaId, Content = "Comentario historia 2", DateComment = DateTime.Now }
                });

            // Act
            var result = await _mockCommentsRepository.Object.GetCommentsByStoriesId(historiaId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, c => Assert.Equal(historiaId, c.IdStorie));
        }

        [Fact]
        public async Task GetCountCommentsByStoriesId_ShouldReturnCorrectCount()
        {
            // Arrange
            var historiaId = 1;

            _mockCommentsRepository.Setup(repo => repo.GetCountCommentsByStoriesId(historiaId))
                .ReturnsAsync(3);

            // Act
            var count = await _mockCommentsRepository.Object.GetCountCommentsByStoriesId(historiaId);

            // Assert
            Assert.Equal(3, count);
        }



        [Fact]
        public async Task AddComment_ShouldAddComment()
        {
            // Arrange
            var comentario = new Comments { Id = 1, Content = "Nuevo comentario", IdUser = 1 };

            // Act
            await _mockCommentsRepository.Object.Add(comentario);

            // Assert
            _mockCommentsRepository.Verify(repo => repo.Add(It.IsAny<Comments>()), Times.Once);
        }

        [Fact]
        public async Task RemoveComment_ShouldRemoveComment()
        {
            // Arrange
            var comentario = new Comments { Id = 1, Content = "Comentario a eliminar", IdUser = 1 };

            // Act
            await _mockCommentsRepository.Object.Remove(comentario);

            // Assert
            _mockCommentsRepository.Verify(repo => repo.Remove(It.IsAny<Comments>()), Times.Once);
        }
    
    }
}
