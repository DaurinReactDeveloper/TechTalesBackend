using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Dtos.VotosHistoriasDto;
using TechTales.Application.Services;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Models;
using TechTales.Persistence.Interfaces;

namespace TechTales.Application.Test
{
    public class VotesStoriesServicesTest
    {

        private readonly Mock<IVotesStories> _mockVotesHistorias;
        private readonly Mock<ILogger<VotesStoriesServices>> _mockLogger;
        private readonly VotesStoriesServices _service;

        public VotesStoriesServicesTest()
        {
            _mockVotesHistorias = new Mock<IVotesStories>();
            _mockLogger = new Mock<ILogger<VotesStoriesServices>>();
            _service = new VotesStoriesServices(_mockVotesHistorias.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetVotesByStoriesId_ReturnsVotes_WhenVotesExist()
        {
            // Arrange
            int storyId = 1;
            int likes = 10;
            int dislikes = 2;

            _mockVotesHistorias.Setup(repo => repo.GetVotesByStoriesId(storyId)).ReturnsAsync((likes, dislikes));

            // Act
            var result = await _service.GetVotesByStoriesId(storyId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Votos obtenidos correctamente.", result.Message);

        }

        [Fact]
        public async Task GetVotesByStoriesId_ReturnsMessage_WhenNoVotesExist()
        {
            // Arrange
            int storyId = 2;

            _mockVotesHistorias.Setup(repo => repo.GetVotesByStoriesId(storyId)).ReturnsAsync((0, 0));

            // Act
            var result = await _service.GetVotesByStoriesId(storyId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("No hay votos.", result.Message);
        }

        [Fact]
        public async Task GetVotesByStoriesId_ReturnsFailure_WhenExceptionOccurs()
        {
            // Arrange
            int storyId = 1;

            _mockVotesHistorias.Setup(repo => repo.GetVotesByStoriesId(storyId))
                .ThrowsAsync(new Exception("Error getting votes"));

            // Act
            var result = await _service.GetVotesByStoriesId(storyId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Ha ocurrido un error obteniendo los votos.", result.Message);
        }

        [Fact]
        public async Task Add_ReturnsSuccess_WhenVotoIsValid()
        {
            // Arrange
            var votoDto = new VotesStoriesAddDto
            {
                Id = 1,
                Vote = true,
                DateVote = DateTime.UtcNow.Date,
                ChangeUser = 1,
                IdStories = 1,
                IdUser = 1

            };

            _mockVotesHistorias.Setup(repo => repo.Add(It.IsAny<VotesStories>())).Returns(Task.CompletedTask);
            _mockVotesHistorias.Setup(repo => repo.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.Add(votoDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Voto agregado correctamente.", result.Message);
        }

        [Fact]
        public async Task Update_ReturnsSuccess_WhenVoteIsUpdated()
        {
            // Arrange
            var existingVote = new VotesStories { Id = 1, Vote = true, DateVote = DateTime.Now };
            var updateDto = new VotesStoriesUpdateDto { Id = 1, Vote = false, ChangeUser = 1 };

            _mockVotesHistorias.Setup(repo => repo.GetById(updateDto.Id)).ReturnsAsync(existingVote);
            _mockVotesHistorias.Setup(repo => repo.Update(It.IsAny<VotesStories>())).Returns(Task.CompletedTask);
            _mockVotesHistorias.Setup(repo => repo.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.Update(updateDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Voto actualizado correctamente", result.Message);
        }

        [Fact]
        public async Task Remove_ReturnsSuccess_WhenVotoExists()
        {
            // Arrange
            var voto = new VotesStories { Id = 1, Vote = false, DateVote = DateTime.Now };
            var removeDto = new VotesStoriesRemoveDto { Id = 1, ChangeUser = 1 };

            _mockVotesHistorias.Setup(repo => repo.GetById(removeDto.Id)).ReturnsAsync(voto);
            _mockVotesHistorias.Setup(repo => repo.Remove(It.IsAny<VotesStories>())).Returns(Task.CompletedTask);
            _mockVotesHistorias.Setup(repo => repo.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.Remove(removeDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Voto eliminado correctamente", result.Message);
        }

        [Fact]
        public async Task Remove_ReturnsFailure_WhenVotoDoesNotExist()
        {
            // Arrange
            var removeDto = new VotesStoriesRemoveDto { Id = 999, ChangeUser = 1 };
            _mockVotesHistorias.Setup(repo => repo.GetById(removeDto.Id)).ReturnsAsync((VotesStories)null);

            // Act
            var result = await _service.Remove(removeDto);

            // Assert
            Assert.False(result.Success);
            Assert.NotEmpty(result.Message);
        }
    }
}
