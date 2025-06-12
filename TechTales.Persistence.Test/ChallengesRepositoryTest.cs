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
    public class ChallengesRepositoryTest
    {
        private readonly Mock<IChallenges> _mockChallengesRepository;
        private readonly Mock<ILogger<ChallengesRepository>> _mockLogger;

        public ChallengesRepositoryTest()
        {
            _mockChallengesRepository = new Mock<IChallenges>();
            _mockLogger = new Mock<ILogger<ChallengesRepository>>();

            _mockChallengesRepository.Setup(repo => repo.GetChallenges()).ReturnsAsync(new List<ChallengesModel>
            {
                new ChallengesModel { Id = 1, Title = "Reto 1", Description = "Descripción Reto 1", DatePublication = DateTime.Now },
                new ChallengesModel { Id = 2, Title = "Reto 2", Description = "Descripción Reto 2", DatePublication = DateTime.Now }
            });

            _mockChallengesRepository.Setup(repo => repo.GetChallengesForType("basic")).ReturnsAsync(new List<ChallengesModel>
            {
                new ChallengesModel { Id = 1, Title = "Reto 1", Description = "Descripción Reto 1", DatePublication = DateTime.Now, Type = "basic" },
            });


            _mockChallengesRepository.Setup(repo => repo.Add(It.IsAny<Challenges>())).Returns(Task.CompletedTask);
            _mockChallengesRepository.Setup(repo => repo.Update(It.IsAny<Challenges>())).Returns(Task.CompletedTask);
            _mockChallengesRepository.Setup(repo => repo.Remove(It.IsAny<Challenges>())).Returns(Task.CompletedTask);

        }

        [Fact]
        public async Task GetChallenge_ShouldReturnAllChallenges()
        {

            var result = await _mockChallengesRepository.Object.GetChallenges();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

        }


        [Fact]
        public async Task GetChallengeForType_ShouldReturnAllChallenges()
        {

            var result = await _mockChallengesRepository.Object.GetChallengesForType("basic");

            Assert.NotNull(result);
            Assert.Equal(1, result.Count);

        }


        [Fact]
        public async Task AddChallenge_ShouldAddNewChallenge()
        {
            var challenge = new Challenges { Id = 1, Title = "Nuevo Reto", Description = "Descripción del reto", DatePublication = DateTime.Now };

            await _mockChallengesRepository.Object.Add(challenge);

            _mockChallengesRepository.Verify(repo => repo.Add(It.IsAny<Challenges>()), Times.Once);

        }

        [Fact]
        public async Task UpdateChallenge_ShouldUpdateExistingChallenge()
        {
            var challenge = new Challenges { Id = 1, Title = "Reto actualizado", Description = "Descripción actualizada" };

            await _mockChallengesRepository.Object.Update(challenge);

            _mockChallengesRepository.Verify(repo => repo.Update(It.IsAny<Challenges>()), Times.Once);

        }

        [Fact]
        public async Task RemoveChallenge_ShouldRemoveChallenge()
        {
            var challenge = new Challenges { Id = 1, Title = "Reto a eliminar", Description = "Descripción a eliminar" };

            await _mockChallengesRepository.Object.Remove(challenge);

            _mockChallengesRepository.Verify(repo => repo.Remove(It.IsAny<Challenges>()), Times.Once);
        }

    }
}
