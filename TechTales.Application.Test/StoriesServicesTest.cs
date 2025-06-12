using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Dtos.HistoriaDto;
using TechTales.Application.Services;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Models;
using TechTales.Persistence.Interfaces;

namespace TechTales.Application.Test
{
    public class StoriesServicesTest
    {

        private readonly Mock<IStories> _storiesMock;
        private readonly Mock<ILogger<StoriesServices>> _loggerMock;
        private readonly StoriesServices _storiesServices;

        public StoriesServicesTest()
        {
            _storiesMock = new Mock<IStories>();
            _loggerMock = new Mock<ILogger<StoriesServices>>();
            _storiesServices = new StoriesServices(_storiesMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetStoriesByUserId_ShouldReturnSuccess_WhenHistoriasAreFound()
        {
            // Arrange
            var userId = 1;
            var storie = new List<StoriesModel>
            {
                new StoriesModel { Id = 1, Title = "Historia 1", Content = "Contenido", IdUser = userId }
            };

            _storiesMock.Setup(x => x.GetStoriesByUserId(userId)).ReturnsAsync(storie);

            // Act
            var result = await _storiesServices.GetStoriesByUserId(userId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Historias del usuario obtenidas correctamente", result.Message);
        }

        [Fact]
        public async Task GetStoriesByUserId_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            var userId = 1;
            _storiesMock.Setup(x => x.GetStoriesByUserId(userId)).ThrowsAsync(new Exception("Error"));

            // Act
            var result = await _storiesServices.GetStoriesByUserId(userId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Error obteniendo las historias del usuario.", result.Message);
        }

        [Fact]
        public async Task Add_ShouldReturnSuccess_WhenHistoriaIsValid()
        {
            // Arrange
            var modelDto = new StoriesAddDto
            {
                Id = 1,
                Title = "Título de prueba",
                Content = "Este es un contenido de prueba que contiene exactamente ciento diez caracteres de longitud para la historia.",
                IdUser = 1,
                DatePublication = DateTime.Now,
            };

            _storiesMock.Setup(x => x.Add(It.IsAny<Stories>())).Returns(Task.CompletedTask);
            _storiesMock.Setup(x => x.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _storiesServices.Add(modelDto);

            // Assert
            //Assert.True(result.Success);
            Assert.Equal("Historia agregada correctamente", result.Message);
        }

        [Fact]
        public async Task Remove_ShouldReturnSuccess_WhenHistoriaIsRemoved()
        {
            // Arrange
            var modelDto = new StoriesRemoveDto { Id = 1, IdUser = 1, ChangeUser = 1 };
            var storie = new Stories { Id = 1, Title = "Historia para eliminar", IdUser = 1 };

            _storiesMock.Setup(x => x.GetById(1)).ReturnsAsync(storie);
            _storiesMock.Setup(x => x.Remove(It.IsAny<Stories>())).Returns(Task.CompletedTask);
            _storiesMock.Setup(x => x.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _storiesServices.Remove(modelDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Historia eliminada correctamente", result.Message);
        }

        [Fact]
        public async Task Remove_ShouldReturnFailure_WhenUserDoesNotHavePermission()
        {
            // Arrange
            var modelDto = new StoriesRemoveDto { Id = 1, IdUser = 2, ChangeUser = 2 }; // ID del usuario incorrecto
            var storie = new Stories { Id = 1, Title = "Historia protegida", IdUser = 1 }; // Dueño original es otro

            _storiesMock.Setup(x => x.GetById(1)).ReturnsAsync(storie);

            // Act
            var result = await _storiesServices.Remove(modelDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("No tiene los permisos necesarios para eliminar esta historia.", result.Message);
        }


        [Fact]
        public async Task Update_ShouldReturnSuccess_WhenHistoriaIsUpdatedSuccessfully()
        {
            // Arrange
            var modelDto = new StoriesUpdateDto
            {
                Id = 1,
                Title = "Título actualizado",
                Content = "Este es un contenido de prueba que contiene exactamente ciento diez caracteres de longitud para la historia.",
                ChangeUser = 1,
                IdUser = 1
            };

            var storie = new Stories
            {
                Id = 1,
                Title = "Título antiguo",
                Content = "Contenido antiguo",
                IdUser = 1
            };

            _storiesMock.Setup(x => x.GetById(modelDto.Id)).ReturnsAsync(storie);
            _storiesMock.Setup(x => x.Update(It.IsAny<Stories>())).Returns(Task.CompletedTask);
            _storiesMock.Setup(x => x.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _storiesServices.Update(modelDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Historia actualizada correctamente", result.Message);
            Assert.Equal(modelDto.Title, storie.Title);
            Assert.Equal(modelDto.Content, storie.Content);
            Assert.Equal(modelDto.ChangeUser, storie.UserMod);
        }

        [Fact]
        public async Task Update_ShouldReturnFailure_WhenUserDoesNotHavePermission()
        {
            // Arrange
            var modelDto = new StoriesUpdateDto
            {
                Id = 1,
                Title = "Título actualizado",
                Content = "Contenido actualizado",
                ChangeUser = 2,
                IdUser = 2 // Usuario diferente al dueño
            };

            var storie = new Stories
            {
                Id = 1,
                Title = "Título antiguo",
                Content = "Contenido antiguo",
                IdUser = 1 // Dueño original
            };

            _storiesMock.Setup(x => x.GetById(modelDto.Id)).ReturnsAsync(storie);

            // Act
            var result = await _storiesServices.Update(modelDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("No tiene los permisos necesarios para eliminar esta historia.", result.Message);
        }

    }
}
