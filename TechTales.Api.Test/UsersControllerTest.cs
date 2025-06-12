using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Api.Controllers;
using TechTales.Application.Contract;
using TechTales.Application.Core;
using TechTales.Application.Dtos.UsuarioDto;
using TechTales.Application.Services;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Models;
using TechTales.Persistence.Interfaces;
using TechTales.Persistence.Repositories;

namespace TechTales.Api.Test
{
    public class UsersControllerTest
    {
        private readonly Mock<IUsersServices> _mockUserService;
        private readonly UsersController _controller;
        private readonly Mock<IConfiguration> _mockConfig;

        public UsersControllerTest()
        {
            _mockUserService = new Mock<IUsersServices>();
            _mockConfig = new Mock<IConfiguration>();
            _controller = new UsersController(_mockUserService.Object, _mockConfig.Object);
        }

        [Fact]
        public async Task GetUsers_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var serviceResult = new ServiceResult { Success = true, Data = "Usuarios encontrados" };
            _mockUserService.Setup(s => s.GetUsers()).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task GetUsers_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var serviceResult = new ServiceResult { Success = false, Message = "Error al obtener usuarios" };
            _mockUserService.Setup(s => s.GetUsers()).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }

        [Fact]
        public async Task GetUser_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password123";
            var serviceResult = new ServiceResult { Success = false, Message = "Error al obtener el usuario" };
            _mockUserService.Setup(s => s.GetUser(email, password, "usuario")).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetUser(email, password);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }


        [Fact]
        public async Task GetUserById_ReturnsOkResult_WhenUserFound()
        {
            // Arrange
            var userId = 1;
            var mockResult = new ServiceResult
            {
                Success = true,
                Data = new { Name = "John Doe", Role = "user" }
            };
            _mockUserService.Setup(service => service.GetUserById(userId)).ReturnsAsync(mockResult);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ServiceResult>(actionResult.Value);
            Assert.True(returnValue.Success);
        }

        [Fact]
        public async Task GetUserAdmin_ReturnsBadRequest_WhenUserNotFound()
        {
            // Arrange
            var email = "admin@example.com";
            var password = "password123";
            var mockResult = new ServiceResult { Success = false, Message = "User not found" };
            _mockUserService.Setup(service => service.GetUser(email, password, "admin")).ReturnsAsync(mockResult);

            // Act
            var result = await _controller.GetUserAdmin(email, password);

            // Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<ServiceResult>(actionResult.Value);
            Assert.False(returnValue.Success);
        }

        [Fact]
        public async Task RegisterWithGoogle_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var usuarioAddDto = new UsersAddDto { Email = "test@example.com", Name = "Test User" };
            var serviceResult = new ServiceResult { Success = false, Message = "Error al registrar el usuario con Google" };
            _mockUserService.Setup(s => s.RegisterWithGoogle(usuarioAddDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.RegisterWithGoogle(usuarioAddDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }

        [Fact]
        public async Task GetUserWithGoogle_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var email = "test@example.com";
            var name = "Test User";
            var serviceResult = new ServiceResult { Success = false, Message = "Error al obtener el usuario con Google" };
            _mockUserService.Setup(s => s.LoginWithGoogle(email, name)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetUserWithGoogle(email, name);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }


        [Fact]
        public async Task PostAdmin_ReturnsOkResult_WhenAdminIsAdded()
        {
            // Arrange
            var userAddDto = new UsersAddDto { Email = "admin@example.com", Name = "Admin User" };
            var mockResult = new ServiceResult
            {
                Success = true,
                Data = new { Name = "Admin User", Role = "admin" }
            };
            _mockUserService.Setup(service => service.AddAdminUser(userAddDto)).ReturnsAsync(mockResult);

            // Act
            var result = await _controller.PostAdmin(userAddDto);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ServiceResult>(actionResult.Value);
            Assert.True(returnValue.Success);
        }

        [Fact]
        public async Task Post_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var usuarioAddDto = new UsersAddDto { Name = "Nuevo Usuario", Role = "usuario" };
            var serviceResult = new ServiceResult { Success = true, Message = "Usuario guardado" };
            _mockUserService.Setup(s => s.Add(usuarioAddDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Post(usuarioAddDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var usuarioAddDto = new UsersAddDto { Name = "Nuevo Usuario", Role = "usuario" };
            var serviceResult = new ServiceResult { Success = false, Message = "Error al guardar el usuario" };
            _mockUserService.Setup(s => s.Add(usuarioAddDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Post(usuarioAddDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var usuarioUpdateDto = new UsersUpdateDto { Id = 1, Name = "Usuario Actualizado" };
            var serviceResult = new ServiceResult { Success = true, Message = "Usuario actualizado" };
            _mockUserService.Setup(s => s.Update(usuarioUpdateDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Put(usuarioUpdateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var usuarioUpdateDto = new UsersUpdateDto { Id = 1, Name = "Usuario Actualizado" };
            var serviceResult = new ServiceResult { Success = false, Message = "Error al actualizar el usuario" };
            _mockUserService.Setup(s => s.Update(usuarioUpdateDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Put(usuarioUpdateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var usuarioRemoveDto = new UsersRemoveDto { Id = 1 };
            var serviceResult = new ServiceResult { Success = true, Message = "Usuario eliminado" };
            _mockUserService.Setup(s => s.Remove(usuarioRemoveDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Delete(usuarioRemoveDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(serviceResult, okResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenFail()
        {
            // Arrange
            var usuarioRemoveDto = new UsersRemoveDto { Id = 1 };
            var serviceResult = new ServiceResult { Success = false, Message = "Error al eliminar el usuario" };
            _mockUserService.Setup(s => s.Remove(usuarioRemoveDto)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.Delete(usuarioRemoveDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }
    }
}
