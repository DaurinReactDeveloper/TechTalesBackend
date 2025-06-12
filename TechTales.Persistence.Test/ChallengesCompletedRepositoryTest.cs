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
    public class ChallengesCompletedRepositoryTest
    {
        private readonly Mock<IChallengesCompleted> _mockChallengeCompletedRepository;
        private readonly Mock<ILogger<ChallengesCompletedRepository>> _mockLogger;


        public ChallengesCompletedRepositoryTest()
        {
            _mockChallengeCompletedRepository = new Mock<IChallengesCompleted>();
            _mockLogger = new Mock<ILogger<ChallengesCompletedRepository>>();

            _mockChallengeCompletedRepository.Setup(repo => repo.GetChallengesCompletedByUserId(It.IsAny<int>())).ReturnsAsync(new List<ChallengesCompletedModel>
            {
                new ChallengesCompletedModel { Id = 1, IdChallenges = 101, DateCompleted = DateTime.Now }
            });


            _mockChallengeCompletedRepository.Setup(repo =>
      repo.VerifyChallengesCompleted(It.IsAny<int>(), It.IsAny<int>()))
      .ReturnsAsync(true);



            _mockChallengeCompletedRepository.Setup(repo => repo.Add(It.IsAny<ChallengesCompleted>())).Returns(Task.CompletedTask);
            _mockChallengeCompletedRepository.Setup(repo => repo.Remove(It.IsAny<ChallengesCompleted>())).Returns(Task.CompletedTask);
        }


        [Fact]
        public async Task GetCompletedRetoByUserId_ShouldReturnCompletedReto()
        {
            var result = await _mockChallengeCompletedRepository.Object.GetChallengesCompletedByUserId(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.Equal(101, result[0].IdChallenges);
        }

        [Fact]
        public async Task VerifyChallengesCompleted_ShouldReturnTrue()
        {
            var result = await _mockChallengeCompletedRepository.Object.VerifyChallengesCompleted(1, 2);

            Assert.True(result); 
        }

        [Fact]
        public async Task AddCompletedReto_ShouldAddNewCompletedReto()
        {
            var challengeCompleted = new ChallengesCompleted { Id = 3, IdChallenges = 103, DateCompleted = DateTime.Now, IdUser = 1 };

            await _mockChallengeCompletedRepository.Object.Add(challengeCompleted);

            _mockChallengeCompletedRepository.Verify(repo => repo.Add(It.IsAny<ChallengesCompleted>()), Times.Once);
        }

        [Fact]
        public async Task RemoveCompletedReto_ShouldRemoveCompletedReto()
        {
            var challengeCompleted = new ChallengesCompleted { Id = 1, IdChallenges = 101, DateCompleted = DateTime.Now, IdUser = 1 };

            await _mockChallengeCompletedRepository.Object.Remove(challengeCompleted);

            _mockChallengeCompletedRepository.Verify(repo => repo.Remove(It.IsAny<ChallengesCompleted>()), Times.Once);
        }

    }
}
