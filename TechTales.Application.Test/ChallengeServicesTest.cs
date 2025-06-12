using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTales.Application.Core;
using TechTales.Application.Dtos.RetoDto;
using TechTales.Application.Services;
using TechTales.Domain.entities;
using TechTales.Persistence.Interfaces;
using Microsoft.Extensions.Logging;
using Xunit;
using TechTales.Infrastructure.Models;
using TechTales.Application.Validations;

namespace TechTales.Application.Test
{
    public class ChallengeServicesTest
    {
        private readonly Mock<IChallenges> _challengeMock;
        private readonly Mock<ILogger<ChallengesServices>> _loggerMock;
        private readonly ChallengesServices _challengeServices;

        public ChallengeServicesTest()
        {
            _challengeMock = new Mock<IChallenges>();
            _loggerMock = new Mock<ILogger<ChallengesServices>>();
            _challengeServices = new ChallengesServices(_challengeMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetRetos_ShouldReturnSuccess_WhenRetosAreFound()
        {
            // Arrange
            var retos = new List<ChallengesModel>
            {
                new ChallengesModel { Id = 1, Title = "Reto 1", Description = "Descripción del reto 1", DatePublication = DateTime.Now }
            };

            _challengeMock.Setup(x => x.GetChallenges()).ReturnsAsync(retos);

            // Act
            var result = await _challengeServices.GetChallenges();

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Retos obtenidos correctamente", result.Message);
        }

        [Fact]
        public async Task GetRetos_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            _challengeMock.Setup(x => x.GetChallenges()).ThrowsAsync(new Exception("Error"));

            // Act
            var result = await _challengeServices.GetChallenges();

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Error obteniendo los retos.", result.Message);
        }

        [Fact]
        public async Task GetChallengesForType_ShouldReturnSuccess_WhenChallengesAreFound()
        {
            // Arrange
            string type = "basic";

            var challengeList = new List<ChallengesModel>
    {
        new ChallengesModel { Id = 1, Title = "Reto básico 1", Description = new string('A', 150), Type = "basic" },
        new ChallengesModel { Id = 2, Title = "Reto básico 2", Description = new string('B', 200), Type = "basic" }
    };

            _challengeMock
                .Setup(x => x.GetChallengesForType(type))
                .ReturnsAsync(challengeList);

            // Act
            var result = await _challengeServices.GetChallengesForType(type);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Retos obtenidos correctamente", result.Message);
            Assert.NotNull(result.Data);
            Assert.IsType<List<ChallengesModel>>(result.Data);
            var data = result.Data as List<ChallengesModel>;
            Assert.Equal(2, data.Count);
        }




        [Fact]
        public async Task Add_ShouldReturnSuccess_WhenRetoIsValid()
        {
            // Arrange
            var modelDto = new ChallengesAddDto
            {
                Id = 1,
                Title = "Reto de prueba del test Add",
                Description = "Este reto tiene exactamente ciento diez caracteres de longitud para validar correctamente su contenido.",
                DatePublication = DateTime.Now,
                Type = "basic",
                Hint = "Pista Prueba: Usa un ciclo for o while para recorrer los números y asegurar que se impriman correctamente.",
                IdAdmin = 1
            };


            _challengeMock.Setup(x => x.Add(It.IsAny<Challenges>())).Returns(Task.CompletedTask);
            _challengeMock.Setup(x => x.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _challengeServices.Add(modelDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Reto agregado correctamente", result.Message);
        }

        [Fact]
        public async Task Remove_ShouldReturnSuccess_WhenRetoIsRemoved()
        {
            // Arrange
            var modelDto = new ChallengesRemoveDto { Id = 1, ChangeUser = 1 };
            var challenge = new Challenges { Id = 1, Title = "Reto a eliminar", Description = "Descripción", DatePublication = DateTime.Now };

            _challengeMock.Setup(x => x.GetById(1)).ReturnsAsync(challenge);
            _challengeMock.Setup(x => x.Remove(It.IsAny<Challenges>())).Returns(Task.CompletedTask);
            _challengeMock.Setup(x => x.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _challengeServices.Remove(modelDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Reto eliminado correctamente", result.Message);
        }

        [Fact]
        public async Task Update_ShouldReturnSuccess_WhenRetoIsUpdatedSuccessfully()
        {
            // Arrange
            var now = DateTime.Now;

            var modelDto = new ChallengesUpdateDto
            {
                Id = 1,
                Title = "Este es un título de reto suficientemente largo",
                Description = new string('D', 150), // 150 caracteres
                Type = "intermediate",
                Hint = "Una pista para resolver el reto",
                ChangeUser = 1
            };

            var challenge = new Challenges
            {
                Id = 1,
                Title = "Título antiguo",
                Description = "Descripción antigua",
                Type = "basic",
                Hint = "Hint antiguo",
                UserMod = 2,
                DatePublication = now.AddDays(-10),
                ModifyDate = now.AddDays(-5)
            };

            _challengeMock.Setup(x => x.GetById(modelDto.Id)).ReturnsAsync(challenge);
            _challengeMock.Setup(x => x.Update(It.IsAny<Challenges>())).Returns(Task.CompletedTask);
            _challengeMock.Setup(x => x.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _challengeServices.Update(modelDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Reto actualizado correctamente", result.Message);

            Assert.Equal(modelDto.Title, challenge.Title);
            Assert.Equal(modelDto.Description, challenge.Description);
            Assert.Equal(modelDto.Type, challenge.Type);
            Assert.Equal(modelDto.Hint, challenge.Hint);
            Assert.Equal(modelDto.ChangeUser, challenge.UserMod);
            Assert.True(challenge.ModifyDate > now);
        }

    }
}
