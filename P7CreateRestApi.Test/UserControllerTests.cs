using AutoMapper;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using P7CreateRestApi.Data.Repositories;
using P7CreateRestApi.Data.Services;
using P7CreateRestApi.Models;
using P7CreateRestApi.Test;
using Xunit;

namespace P7CreateRestApi.Tests
{
    public class UserControllerTests
    {
        private readonly LocalDbTestContext _dbContext;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPasswordHasher<IdentityUser>> _passwordHasherMock;
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            // Configurer une base de données en mémoire
            var options = new DbContextOptionsBuilder<LocalDbTestContext>()
                .UseInMemoryDatabase(databaseName: "LocalDbTest")
                .Options;

            _dbContext = new LocalDbTestContext(options);
            _userServiceMock = new Mock<IUserService>();
            _mapperMock = new Mock<IMapper>();
            _passwordHasherMock = new Mock<IPasswordHasher<IdentityUser>>();

            // Configurer UserManager Mock
            var storeMock = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                storeMock.Object, null, null, null, null, null, null, null, null
            );

            // Initialiser le contrôleur
            _controller = new UserController(_userServiceMock.Object, _mapperMock.Object, _passwordHasherMock.Object, _userManagerMock.Object);
        }

        #region ----- Create Tests -----

        [Fact]
        public async Task Create_ShouldReturnOk_WhenUserIsCreated()
        {
            // Arrange
            var model = new UserViewModel
            {
                Username = "TestUser",
                Password = "TestPassword"
            };

            var identityUser = new IdentityUser { UserName = model.Username };

            _passwordHasherMock
                .Setup(p => p.HashPassword(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .Returns("HashedPassword");

            _userManagerMock
                .Setup(u => u.CreateAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.Create(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User created successfully.", okResult.Value);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Username", "Username is required.");

            var model = new UserViewModel();

            // Act
            var result = await _controller.Create(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenUserManagerFails()
        {
            // Arrange
            var model = new UserViewModel
            {
                Username = "TestUser",
                Password = "TestPassword"
            };

            _passwordHasherMock
                .Setup(p => p.HashPassword(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .Returns("HashedPassword");

            _userManagerMock
                .Setup(u => u.CreateAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User creation failed." }));

            // Act
            var result = await _controller.Create(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        #endregion

        #region ----- GetEntity Tests -----
        [Fact]
        public void GetEntity_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, Username = "TestUser" };
            _userServiceMock.Setup(s => s.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(user);

            var userViewModel = new UserViewModel { Username = "TestUser" };
            _mapperMock.Setup(m => m.Map<UserViewModel>(It.IsAny<User>())).Returns(userViewModel);

            // Act
            var result = _controller.GetEntity(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserViewModel>(okResult.Value);
            Assert.Equal("TestUser", returnedUser.Username);
        }

        [Fact]
        public void GetEntity_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _userServiceMock.Setup(s => s.FindByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act
            var result = _controller.GetEntity(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        #endregion

        #region ----- UpdateUser Tests -----

        [Fact]
        public async Task UpdateUser_ShouldReturnOk_WhenUserIsUpdated()
        {
            // Arrange
            int userId = 1;
            var userViewModel = new UserViewModel
            {
                
                Username = "UpdatedUser",
                Password = "UpdatedPassword"
            };

            var existingUser = new User
            {
                Id = userId,
                Username = "OldUser",
                Password = "OldPassword"
            };

            _userServiceMock.Setup(s => s.FindByIdAsync(userId))
                .ReturnsAsync(existingUser);

            _userServiceMock.Setup(s => s.UpdateAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask); // Simule une mise à jour réussie.

            _mapperMock.Setup(m => m.Map(userViewModel, existingUser))
                .Returns(existingUser); // Simule le mapping inverse.

            _mapperMock.Setup(m => m.Map<UserViewModel>(existingUser))
                .Returns(userViewModel); // Simule le mapping retour.

            // Act
            var result = await _controller.UpdateUser(userId, userViewModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedModel = Assert.IsType<UserViewModel>(okResult.Value);

            Assert.Equal(userViewModel.Username, returnedModel.Username);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _userServiceMock.Setup(s => s.FindByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

            var model = new UserViewModel { Username = "UpdatedUser" };

            // Act
            var result = await _controller.UpdateUser(1, model);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        #endregion

        #region ----- DeleteUser Tests -----
        [Fact]
        public async Task DeleteUser_ShouldReturnNoContent_WhenUserIsDeleted()
        {
            // Arrange
            var user = new User { Id = 1, Username = "TestUser" };

            _userServiceMock.Setup(s => s.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _userServiceMock.Setup(s => s.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _userServiceMock.Setup(s => s.FindByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        #endregion
    }
}
