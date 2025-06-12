using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTales.Application.Contract;
using TechTales.Application.Dtos.UsuarioDto;
using TechTales.Application.Services;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Models;
using TechTales.Persistence.Interfaces;
using Xunit;

namespace TechTales.Application.Test
{
    public class UsersServicesTests
    {
        private readonly Mock<IUsers> _mockUserRepository;
        private readonly Mock<ILogger<UsersServices>> _mockLogger;
        private readonly Mock<INotificationsServices> _mockEmailServices;
        private readonly Mock<ICloudinaryServices> _mockCloudinaryServices;
        private readonly Mock<IPasswordHelperServices> _mockPasswordHelperServices;
        private readonly UsersServices _userService;

        public UsersServicesTests()
        {
            _mockUserRepository = new Mock<IUsers>();
            _mockLogger = new Mock<ILogger<UsersServices>>();
            _mockEmailServices = new Mock<INotificationsServices>();
            _mockCloudinaryServices = new Mock<ICloudinaryServices>();
            _mockPasswordHelperServices = new Mock<IPasswordHelperServices>();

            _userService = new UsersServices(
                _mockUserRepository.Object,
                _mockLogger.Object,
                _mockEmailServices.Object,
                _mockCloudinaryServices.Object,
                _mockPasswordHelperServices.Object
            );
        }

        [Fact]
        public async Task GetUsers_ShouldReturnUsers_WhenUsersExist()
        {
            // Arrange
            var users = new List<UsersModel>
        {
            new UsersModel { Id = 1, Name = "Test User", Email = "test@example.com" }
        };

            _mockUserRepository.Setup(repo => repo.GetUsers()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetUsers();

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Usuarios obtenidos correctamente", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(1, ((List<UsersModel>)result.Data).Count);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnError_WhenNoUsersFound()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetUsers()).ReturnsAsync(new List<UsersModel>());

            // Act
            var result = await _userService.GetUsers();

            // Assert
            Assert.False(result.Success);
            Assert.Equal("No se encontraron los usuarios disponibles.", result.Message);
        }

        [Fact]
        public async Task Add_ShouldReturnSuccess_WhenUserIsAdded()
        {
            // Arrange
            var model = new UsersAddDto
            {
                Name = "New User 1",
                Email = "newuser@example.com",
                PasswordHash = "password123",
                ImgProfile = "http://example.com/image.jpg"
            };

            _mockUserRepository.Setup(repo => repo.GetUserEmail(model.Email)).ReturnsAsync((UsersModel)null);
            _mockCloudinaryServices.Setup(cloud => cloud.UploadImageFromUrlAsync(It.IsAny<string>())).ReturnsAsync("http://cloudinary.com/image.jpg");
            _mockUserRepository.Setup(repo => repo.Add(It.IsAny<Users>())).Returns(Task.CompletedTask);

            // Act
            var result = await _userService.Add(model);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Usuario agregado correctamente", result.Message);
        }


        [Fact]
        public async Task Remove_ShouldReturnSuccess_WhenUserIsRemoved()
        {
            // Arrange
            var modelDto = new UsersRemoveDto { Id = 1, ChangeUser = 1 };
            var existingUser = new Users { Id = 1, Name = "Test User", Email = "test@example.com", UserDeleted = null };

            _mockUserRepository.Setup(repo => repo.GetById(modelDto.Id)).ReturnsAsync(existingUser);
            _mockUserRepository.Setup(repo => repo.Remove(It.IsAny<Users>())).Returns(Task.CompletedTask);

            // Act
            var result = await _userService.Remove(modelDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Usuario eliminado correctamente", result.Message);
        }

        [Fact]
        public async Task Update_ShouldReturnSuccess_WhenUserIsUpdated()
        {
            // Arrange
            var modelDto = new UsersUpdateDto { Id = 1, Name = "Updated User", Email = "updated@example.com", PasswordHash = "abc2015as" };
            var existingUser = new Users { Id = 1, Name = "Test User", Email = "test@example.com" };

            _mockUserRepository.Setup(repo => repo.GetById(modelDto.Id)).ReturnsAsync(existingUser);
            _mockUserRepository.Setup(repo => repo.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _userService.Update(modelDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Usuario actualizado correctamente", result.Message);
        }

        [Fact]
        public async Task RegisterWithGoogle_ShouldReturnError_WhenEmailExists()
        {
            // Arrange
            var modelDto = new UsersAddDto { Email = "test@example.com", Name = "Test User", ImgProfile = "profileUrl" };
            _mockUserRepository.Setup(x => x.GetUserEmail(It.IsAny<string>())).ReturnsAsync(new UsersModel());

            // Act
            var result = await _userService.RegisterWithGoogle(modelDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El correo electrónico ya está registrado.", result.Message);
        }

        [Fact]
        public async Task AddAdminUser_ShouldReturnError_WhenEmailExists()
        {
            // Arrange
            var modelDto = new UsersAddDto { Email = "admin@example.com", Name = "Admin User", ImgProfile = "profileUrl", PasswordHash = "password" };
            _mockUserRepository.Setup(x => x.GetUserEmail(It.IsAny<string>())).ReturnsAsync(new UsersModel());

            // Act
            var result = await _userService.AddAdminUser(modelDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El correo electrónico ya está registrado.", result.Message);
        }

        [Fact]
        public async Task AddAdminUser_ShouldReturnSuccess_WhenAdminUserIsAdded()
        {
            // Arrange
            var modelDto = new UsersAddDto { Email = "admin@example.com", Name = "Admin User", ImgProfile = "profileUrl", PasswordHash = "abc2015as" };
            _mockUserRepository.Setup(x => x.GetUserEmail(It.IsAny<string>())).ReturnsAsync((UsersModel)null);
            _mockCloudinaryServices.Setup(x => x.UploadImageFromUrlAsync(It.IsAny<string>())).ReturnsAsync("imageUrl");
            _mockPasswordHelperServices.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("hashedPassword");

            // Act
            var result = await _userService.AddAdminUser(modelDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Usuario agregado correctamente", result.Message);
        }

        [Fact]
        public async Task GetUser_ShouldReturnError_WhenUserEmailIsInvalid()
        {
            // Arrange
            string email = "test@example.com";
            string password = "password123";
            string role = "admin";

            _mockUserRepository.Setup(repo => repo.GetUserEmail(email)).ReturnsAsync((UsersModel)null);

            // Act
            var result = await _userService.GetUser(email, password, role);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Email Incorrecto", result.Message);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnSuccess_WhenUserIsFound()
        {
            // Arrange
            int id = 1;

            var user = new UsersModel
            {
                Id = 1,
                Name = "Test User",
                Email = "test@example.com"
            };

            _mockUserRepository.Setup(repo => repo.GetUserById(id)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserById(id);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Usuario Obtenido Correctamente", result.Message);
            Assert.NotNull(result.Data);
        }

    }
}
