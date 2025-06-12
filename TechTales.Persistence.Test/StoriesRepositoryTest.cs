using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Models;
using TechTales.Persistence.Interfaces;
using TechTales.Persistence.Repositories;

namespace TechTales.Persistence.Test
{
    public class StoriesRepositoryTest
    {

        private readonly Mock<IStories> _mockStories;
        private readonly Mock<ILogger<StoriesRepository>> _mockLogger;

        public StoriesRepositoryTest()
        {

            _mockStories = new Mock<IStories>();
            _mockLogger = new Mock<ILogger<StoriesRepository>>();

            _mockStories.Setup(repo => repo.GetStoriesByUserId(It.IsAny<int>()))
                .ReturnsAsync((int usuarioId) =>
                {
                    if (usuarioId == 1)
                    {
                        return new List<StoriesModel>
                        {
                            new StoriesModel { Id = 1, Title = "Primer historia", DatePublication = DateTime.Now }
                        };
                    }
                    return new List<StoriesModel>
                    {
                        new StoriesModel { Id = 1, Title = "Primer historia", DatePublication = DateTime.Now },
                        new StoriesModel { Id = 2, Title = "Segunda historia", DatePublication = DateTime.Now }
                    };
                });

            _mockStories.Setup(repo => repo.Add(It.IsAny<Stories>())).Returns(Task.CompletedTask);
            _mockStories.Setup(repo => repo.Update(It.IsAny<Stories>())).Returns(Task.CompletedTask);
            _mockStories.Setup(repo => repo.Remove(It.IsAny<Stories>())).Returns(Task.CompletedTask);

        }

        [Fact]
        public async Task GetStoriesByUserId_ShouldReturnHistoryForUserId()
        {

            int usuarioId = 1;

            var result = await _mockStories.Object.GetStoriesByUserId(usuarioId);

            Assert.NotNull(result);
            Assert.Single(result); // Solo una historia esperada
            Assert.Equal("Primer historia", result[0].Title);
        }

        [Fact]
        public async Task GetStories_ShouldReturnAllStories()
        {
            // Arrange
            _mockStories.Setup(repo => repo.GetStories())
                .ReturnsAsync(new List<StoriesModel>
                {
            new StoriesModel { Id = 1, Title = "Historia 1", DatePublication = DateTime.Now },
            new StoriesModel { Id = 2, Title = "Historia 2", DatePublication = DateTime.Now }
                });

            // Act
            var result = await _mockStories.Object.GetStories();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Historia 1", result[0].Title);
            Assert.Equal("Historia 2", result[1].Title);
        }

        [Fact]
        public async Task AddStorie_ShouldAddStorie()
        {

            var historia = new Stories { Id = 1, Title = "Nueva historia", IdUser = 1 };

            await _mockStories.Object.Add(historia);

            _mockStories.Verify(repo => repo.Add(It.IsAny<Stories>()), Times.Once);

        }

        [Fact]
        public async Task UpdateStorie_ShouldUpdateStorie()
        {

            var historia = new Stories { Id = 1, Title = "Historia actualizada", IdUser = 1 };

            await _mockStories.Object.Update(historia);

            _mockStories.Verify(repo => repo.Update(It.IsAny<Stories>()), Times.Once);

        }

        [Fact]
        public async Task RemoveStorie_ShouldRemoveStorie()
        {

            var historia = new Stories { Id = 1, Title = "Historia a eliminar", IdUser = 1 };

            await _mockStories.Object.Remove(historia);

            _mockStories.Verify(repo => repo.Remove(It.IsAny<Stories>()), Times.Once);

        }

    }
}
