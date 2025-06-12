using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Exceptions;
using TechTales.Infrastructure.Models;
using TechTales.Persistence.Context;
using TechTales.Persistence.Interfaces;
using TechTales.Persistence.Repositories;
using Xunit;

namespace TechTales.Persistence.Test
{
    public class UserRepositoryTest
    {
        private readonly Mock<IUsers> _mockUserRepository;
        private readonly Mock<ILogger<UsersRepository>> _mockLogger;
        private readonly Mock<DbtechtalesContext> _mockDbContext;

        public UserRepositoryTest()
        {
            _mockUserRepository = new Mock<IUsers>();
            _mockLogger = new Mock<ILogger<UsersRepository>>();
            _mockDbContext = new Mock<DbtechtalesContext>();

            _mockUserRepository.Setup(repo => repo.GetUserEmail(It.IsAny<string>())).ReturnsAsync(new UsersModel
            {
                Name = "Test User",
                Email = "test@domain.com",
                PasswordHash = "hashedpassword",
                Role = "user"
            });

            _mockUserRepository.Setup(repo => repo.GetUserLogin(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new UsersModel
            {
                Name = "Test User",
                Email = "test@domain.com",
                PasswordHash = "hashedpassword",
                Role = "user"
            });

            _mockUserRepository.Setup(repo => repo.GetUserLogin(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new UsersModel
            {
                Name = "Test User",
                Email = "test@domain.com",
                PasswordHash = "hashedpassword",
                Role = "admin"
            });

            _mockUserRepository.Setup(repo => repo.GetUsers()).ReturnsAsync(new List<UsersModel>
            {
                new UsersModel { Id = 1, Name = "Test User", Email = "test@domain.com", PasswordHash = "hashedpassword", DateRegister = DateTime.Now }
            });

            _mockUserRepository.Setup(repo => repo.Add(It.IsAny<Users>())).Returns(Task.CompletedTask);
            _mockUserRepository.Setup(repo => repo.Update(It.IsAny<Users>())).Returns(Task.CompletedTask);
            _mockUserRepository.Setup(repo => repo.Remove(It.IsAny<Users>())).Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task GetUser_ShouldReturnUserDetails()
        {
            var result = await _mockUserRepository.Object.GetUserLogin("test@domain.com", "hashedpassword", "user");

            Assert.NotNull(result);
            Assert.Equal("Test User", result.Name);
            Assert.Equal("test@domain.com", result.Email);
        }

        [Fact]
        public async Task GetUserAdmin_ShouldReturnUserAdminDetails()
        {
            var result = await _mockUserRepository.Object.GetUserLogin("test@domain.com", "hashedpassword", "admin");

            Assert.NotNull(result);
            Assert.Equal("Test User", result.Name);
            Assert.Equal("test@domain.com", result.Email);
        }

        [Fact]
        public async Task GetUserByEmail_ShouldReturnUserDetails()
        {
            var result = await _mockUserRepository.Object.GetUserEmail("test@domain.com");

            Assert.NotNull(result);
            Assert.Equal("Test User", result.Name);
            Assert.Equal("test@domain.com", result.Email);
        }

        [Fact]
        public async Task GetUsersList_ShouldReturnUserList()
        {
            var result = await _mockUserRepository.Object.GetUsers();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Test User", result[0].Name);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUserDetails()
        {
            // Arrange
            var userId = 1;
            var expectedUser = new UsersModel
            {
                Name = "Test User",
                ImgProfile = "profile.jpg"
            };

            _mockUserRepository.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(expectedUser);

            // Act
            var result = await _mockUserRepository.Object.GetUserById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test User", result.Name);
            Assert.Equal("profile.jpg", result.ImgProfile);
        }


        [Fact]
        public async Task AddUser_ShouldAddNewUser()
        {
            var user = new Users { Id = 1, Name = "New User", Email = "newuser@domain.com", PasswordHash = "newhashedpassword", Role = "Admin" };

            await _mockUserRepository.Object.Add(user);

            _mockUserRepository.Verify(repo => repo.Add(It.IsAny<Users>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_ShouldUpdateUserDetails()
        {
            var user = new Users { Id = 1, Name = "Updated User", Email = "updated@domain.com", PasswordHash = "updatedpassword", Role = "User" };

            await _mockUserRepository.Object.Update(user);

            _mockUserRepository.Verify(repo => repo.Update(It.IsAny<Users>()), Times.Once);
        }

        [Fact]
        public async Task RemoveUser_ShouldRemoveUser()
        {
            var user = new Users { Id = 1, Name = "User to Remove", Email = "remove@domain.com", PasswordHash = "removehashedpassword", Role = "User" };

            await _mockUserRepository.Object.Remove(user);

            _mockUserRepository.Verify(repo => repo.Remove(It.IsAny<Users>()), Times.Once);
        }

    }
}
