using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Exceptions;
using TechTales.Infrastructure.Models;
using TechTales.Persistence.Interfaces;
using TechTales.Persistence.Repositories;
using Xunit;

namespace TechTales.Persistence.Test
{
    public class VotesStoriesRepositoryTest
    {
        private readonly Mock<IVotesStories> _mockVotesStoriesRepository;
        private readonly Mock<ILogger<VotesStoriesRepository>> _mockLogger;

        public VotesStoriesRepositoryTest()
        {
            _mockVotesStoriesRepository = new Mock<IVotesStories>();
            _mockLogger = new Mock<ILogger<VotesStoriesRepository>>();

            _mockVotesStoriesRepository.Setup(repo => repo.Add(It.IsAny<VotesStories>())).Returns(Task.CompletedTask);
            _mockVotesStoriesRepository.Setup(repo => repo.Update(It.IsAny<VotesStories>())).Returns(Task.CompletedTask);
            _mockVotesStoriesRepository.Setup(repo => repo.Remove(It.IsAny<VotesStories>())).Returns(Task.CompletedTask);
        }


        [Fact]
        public async Task GetVotesByStoriesId_ShouldThrowVotosHistoriasExceptions_WhenErrorOccurs()
        {
            _mockVotesStoriesRepository.Setup(repo => repo.GetVotesByStoriesId(It.IsAny<int>())).ThrowsAsync(new VotesStoriesExceptions("Error"));

            await Assert.ThrowsAsync<VotesStoriesExceptions>(() => _mockVotesStoriesRepository.Object.GetVotesByStoriesId(1));
        }

        [Fact]
        public async Task GetVotesByUserAndStories_ShouldReturnVoteDetails()
        {

            var vote = new VotesStories { Id = 1, IdStories = 2, IdUser = 1, Vote = true };
            _mockVotesStoriesRepository.Setup(repo => repo.GetVotesByUserAndStories(1, 2)).ReturnsAsync(vote);

            var result = await _mockVotesStoriesRepository.Object.GetVotesByUserAndStories(1, 2);

            Assert.NotNull(result);
            Assert.Equal(1, result.IdUser);
            Assert.Equal(2, result.IdStories);
            Assert.True(result.Vote);
        }


        [Fact]
        public async Task GetVotesByUserAndStoriesTrueDelete_ShouldReturnVoteDetails_WhenVoteExists()
        {

            var vote = new VotesStories { IdUser = 1, IdStories = 2, Vote = true };

            _mockVotesStoriesRepository.Setup(repo => repo.GetVotesByUserAndStoriesTrueDelete(1, 2)).ReturnsAsync(vote);

            var result = await _mockVotesStoriesRepository.Object.GetVotesByUserAndStoriesTrueDelete(1, 2);

            Assert.NotNull(result);
            Assert.Equal(1, result.IdUser);
            Assert.Equal(2, result.IdStories);
            Assert.True(result.Vote);
        }

        [Fact]
        public async Task GetVotesByUserAndStoriesTrueDelete_ShouldThrowVotesStoriesExceptions_WhenErrorOccurs()
        {

            _mockVotesStoriesRepository.Setup(repo => repo.GetVotesByUserAndStoriesTrueDelete(It.IsAny<int>(), It.IsAny<int>()))
                                       .ThrowsAsync(new VotesStoriesExceptions("Error"));

            await Assert.ThrowsAsync<VotesStoriesExceptions>(() => _mockVotesStoriesRepository.Object.GetVotesByUserAndStoriesTrueDelete(1, 2));
        }



        [Fact]
        public async Task AddVote_ShouldInvokeAddMethodOnce()
        {
            var voto = new VotesStories { IdStories = 1, Vote = true };

            await _mockVotesStoriesRepository.Object.Add(voto);

            _mockVotesStoriesRepository.Verify(repo => repo.Add(It.IsAny<VotesStories>()), Times.Once);
        }

        [Fact]
        public async Task UpdateVote_ShouldInvokeUpdateMethodOnce()
        {
            var voto = new VotesStories { Id = 1, Vote = false };

            await _mockVotesStoriesRepository.Object.Update(voto);

            _mockVotesStoriesRepository.Verify(repo => repo.Update(It.IsAny<VotesStories>()), Times.Once);
        }

        [Fact]
        public async Task RemoveVote_ShouldInvokeRemoveMethodOnce()
        {
            var voto = new VotesStories { Id = 1 };

            await _mockVotesStoriesRepository.Object.Remove(voto);

            _mockVotesStoriesRepository.Verify(repo => repo.Remove(It.IsAny<VotesStories>()), Times.Once);
        }

        [Fact]
        public async Task GetVotesByStoriesId_ShouldThrowVotosHistoriasExceptions()
        {
            _mockVotesStoriesRepository.Setup(repo => repo.GetVotesByStoriesId(It.IsAny<int>())).ThrowsAsync(new VotesStoriesExceptions("Error"));

            await Assert.ThrowsAsync<VotesStoriesExceptions>(() => _mockVotesStoriesRepository.Object.GetVotesByStoriesId(1));
        }
    }
}
