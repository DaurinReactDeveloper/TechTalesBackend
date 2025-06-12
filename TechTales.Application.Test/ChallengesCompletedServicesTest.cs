using Moq;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TechTales.Application.Core;
using TechTales.Application.Dtos.RetosCompletados;
using TechTales.Application.Services;
using TechTales.Persistence.Interfaces;
using Xunit;
using TechTales.Infrastructure.Models;
using TechTales.Domain.entities;

namespace TechTales.Application.Test
{
    public class ChallengesCompletedServicesTest
    {
        private readonly Mock<IChallengesCompleted> _mockChallengesCompleted;
        private readonly Mock<ILogger<ChallengesCompletedServices>> _mockLogger;
        private readonly ChallengesCompletedServices _service;

        public ChallengesCompletedServicesTest()
        {
            _mockChallengesCompleted = new Mock<IChallengesCompleted>();
            _mockLogger = new Mock<ILogger<ChallengesCompletedServices>>();
            _service = new ChallengesCompletedServices(_mockChallengesCompleted.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetChallengesCompletedByUserId_ShouldReturnChallenges_WhenUserHasCompletedChallenges()
        {
            // Arrange
            var userId = 1;
            var challengesCompletedList = new List<ChallengesCompletedModel>
    {
        new ChallengesCompletedModel { Id = 1, IdChallenges = 1, IdUser = userId },
        new ChallengesCompletedModel { Id = 2, IdChallenges = 2, IdUser = userId }
    };

            // Configuración de Moq para devolver una lista de ChallengesCompletedModel
            _mockChallengesCompleted.Setup(x => x.GetChallengesCompletedByUserId(userId))
                                    .ReturnsAsync(challengesCompletedList);

            // Act
            var result = await _service.GetChallengesCompletedByUserId(userId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Retos Completados del usuario obtenidos correctamente", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(2, ((List<ChallengesCompletedModel>)result.Data).Count);
        }

        [Fact]
        public async Task GetChallengesCompletedByUserId_ShouldReturnError_WhenExceptionOccurs()
        {
            // Arrange
            var userId = 1;
            _mockChallengesCompleted.Setup(x => x.GetChallengesCompletedByUserId(userId)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _service.GetChallengesCompletedByUserId(userId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Error obteniendo los retos completados del usuario.", result.Message);
        }

        [Fact]
        public async Task Add_ShouldReturnSuccess_WhenRetoIsAddedSuccessfully()
        {
            // Arrange
            var modelDto = new ChallengesCompletedAddDto
            {
                Id = 1,
                IdChallenges = 1,
                IdUser = 1,
                ChangeUser = 1
            };

            _mockChallengesCompleted.Setup(x => x.Add(It.IsAny<Domain.entities.ChallengesCompleted>())).Returns(Task.CompletedTask);
            _mockChallengesCompleted.Setup(x => x.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.Add(modelDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Reto Completado agregado correctamente", result.Message);
        }

        [Fact]
        public async Task Add_ShouldReturnError_WhenValidationFails()
        {
            // Arrange
            var modelDto = new ChallengesCompletedAddDto
            {
                Id = 1,
                IdChallenges = 0,
                IdUser = 1,
                ChangeUser = 1
            };

            // Act
            var result = await _service.Add(modelDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El ID del reto debe ser un valor mayor a 0.", result.Message);
        }

        [Fact]
        public async Task Add_ShouldReturnError_WhenExceptionOccurs()
        {
            // Arrange
            var modelDto = new ChallengesCompletedAddDto
            {
                Id = 1,
                IdChallenges = 1,
                IdUser = 1,
                ChangeUser = 1
            };

            _mockChallengesCompleted.Setup(x => x.Add(It.IsAny<Domain.entities.ChallengesCompleted>())).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _service.Add(modelDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Ha ocurrido un error guardando el reto completado.", result.Message);
        }

        [Fact]
        public async Task Remove_ShouldReturnSuccess_WhenRetoIsRemovedSuccessfully()
        {
            // Arrange
            var modelDto = new ChallengesCompletedRemoveDto { Id = 1, ChangeUser = 1, IdChallenges = 1 };
            var challengeCompleted = new Domain.entities.ChallengesCompleted { Id = 1 };
            _mockChallengesCompleted.Setup(x => x.GetById(modelDto.Id)).ReturnsAsync(challengeCompleted);
            _mockChallengesCompleted.Setup(x => x.Remove(It.IsAny<Domain.entities.ChallengesCompleted>())).Returns(Task.CompletedTask);
            _mockChallengesCompleted.Setup(x => x.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.Remove(modelDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Reto Completado eliminado correctamente", result.Message);
        }

        [Fact]
        public async Task Remove_ShouldReturnError_WhenRetoNotFound()
        {
            // Arrange
            var modelDto = new ChallengesCompletedRemoveDto { Id = 1, ChangeUser = 1 };
            _mockChallengesCompleted.Setup(x => x.GetById(modelDto.Id)).ReturnsAsync((Domain.entities.ChallengesCompleted)null);

            // Act
            var result = await _service.Remove(modelDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("No se pudo obtener el reto completado.", result.Message);
        }

        [Fact]
        public async Task Remove_ShouldReturnError_WhenValidationFails()
        {
            // Arrange
            var modelDto = new ChallengesCompletedRemoveDto { Id = 1, ChangeUser = 1, IdChallenges = 1 };
            var challengeCompleted = new Domain.entities.ChallengesCompleted { Id = 1 };
            _mockChallengesCompleted.Setup(x => x.GetById(modelDto.Id)).ReturnsAsync(challengeCompleted);
            _mockChallengesCompleted.Setup(x => x.Remove(It.IsAny<Domain.entities.ChallengesCompleted>())).Returns(Task.CompletedTask);
            _mockChallengesCompleted.Setup(x => x.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.Remove(modelDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Reto Completado eliminado correctamente", result.Message);
        }

    }
}
