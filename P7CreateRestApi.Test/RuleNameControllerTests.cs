using AutoMapper;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;
using P7CreateRestApi.Data.Services;
using System.Threading.Tasks;
using Xunit;

namespace P7CreateRestApi.Tests
{
    public class RuleNameControllerTests
    {
        private readonly Mock<IRuleNameService> _ruleNameServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly RuleNameController _controller;

        public RuleNameControllerTests()
        {
            _ruleNameServiceMock = new Mock<IRuleNameService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new RuleNameController(_ruleNameServiceMock.Object, _mapperMock.Object);
        }

        #region Create

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenRuleNameIsCreated()
        {
            // Arrange
            var ruleNameViewModel = new RuleNameViewModel
            {
                Name = "Test Rule",
                Description = "Test Description",
                Json = "{}",
                Template = "Template",
                SqlStr = "SELECT *",
                SqlPart = "FROM Test"
            };

            var ruleName = new RuleName
            {
                Id = 1,
                Name = "Test Rule",
                Description = "Test Description",
                Json = "{}",
                Template = "Template",
                SqlStr = "SELECT *",
                SqlPart = "FROM Test"
            };

            _mapperMock.Setup(m => m.Map<RuleName>(ruleNameViewModel)).Returns(ruleName);
            _ruleNameServiceMock.Setup(s => s.AddAsync(ruleName)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(ruleNameViewModel);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.Create), createdResult.ActionName);
            Assert.Equal(ruleName.Id, ((RuleName)createdResult.Value).Id);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        #endregion

        #region Read

        [Fact]
        public async Task GetEntity_ShouldReturnOk_WhenRuleNameIsFound()
        {
            // Arrange
            int id = 1;
            var ruleName = new RuleName
            {
                Id = id,
                Name = "Test Rule",
                Description = "Test Description",
                Json = "{}",
                Template = "Template",
                SqlStr = "SELECT *",
                SqlPart = "FROM Test"
            };

            var ruleNameViewModel = new RuleNameViewModel
            {
                Name = "Test Rule",
                Description = "Test Description",
                Json = "{}",
                Template = "Template",
                SqlStr = "SELECT *",
                SqlPart = "FROM Test"
            };

            _ruleNameServiceMock.Setup(s => s.FindByIdAsync(id)).ReturnsAsync(ruleName);
            _mapperMock.Setup(m => m.Map<RuleNameViewModel>(ruleName)).Returns(ruleNameViewModel);

            // Act
            var result = await _controller.GetEntity(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(ruleNameViewModel, okResult.Value);
        }

        [Fact]
        public async Task GetEntity_ShouldReturnNotFound_WhenRuleNameIsNotFound()
        {
            // Arrange
            int id = 1;
            _ruleNameServiceMock.Setup(s => s.FindByIdAsync(id)).ReturnsAsync((RuleName)null);

            // Act
            var result = await _controller.GetEntity(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateRuleName_ShouldReturnOk_WhenRuleNameIsUpdated()
        {
            // Arrange
            int id = 1;
            var ruleNameViewModel = new RuleNameViewModel
            {
                Name = "Updated Rule",
                Description = "Updated Description",
                Json = "{}",
                Template = "Updated Template",
                SqlStr = "SELECT *",
                SqlPart = "FROM Updated"
            };

            var existingRuleName = new RuleName
            {
                Id = id,
                Name = "Old Rule",
                Description = "Old Description",
                Json = "{}",
                Template = "Old Template",
                SqlStr = "SELECT *",
                SqlPart = "FROM Old"
            };

            _ruleNameServiceMock.Setup(s => s.FindByIdAsync(id)).ReturnsAsync(existingRuleName);
            Assert.NotNull(existingRuleName); // Vérifier que l'objet existe

            // 🔴 Correction : S'assurer que le mapping met bien à jour existingRuleName
            _mapperMock.Setup(m => m.Map(It.IsAny<RuleNameViewModel>(), It.IsAny<RuleName>()))
                .Callback<RuleNameViewModel, RuleName>((src, dest) =>
                {
                    dest.Name = src.Name;
                    dest.Description = src.Description;
                    dest.Json = src.Json;
                    dest.Template = src.Template;
                    dest.SqlStr = src.SqlStr;
                    dest.SqlPart = src.SqlPart;
                });

            // 🔴 Correction : S'assurer que UpdateAsync sauvegarde bien l'objet modifié
            _ruleNameServiceMock.Setup(s => s.UpdateAsync(It.IsAny<RuleName>()))
                .Callback<RuleName>(r => existingRuleName = r) // Sauvegarde l'objet mis à jour
                .Returns(Task.CompletedTask);

            _mapperMock.Setup(m => m.Map<RuleNameViewModel>(It.IsAny<RuleName>()))
                       .Returns((RuleName r) => new RuleNameViewModel
                       {
                           Name = r.Name,
                           Description = r.Description,
                           Json = r.Json,
                           Template = r.Template,
                           SqlStr = r.SqlStr,
                           SqlPart = r.SqlPart
                       });

            // Act
            var result = await _controller.UpdateRuleName(id, ruleNameViewModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedRuleName = Assert.IsType<RuleNameViewModel>(okResult.Value);

            // 🔴 Vérification après mise à jour
            Assert.Equal(ruleNameViewModel.Name, updatedRuleName.Name);
            Assert.Equal(ruleNameViewModel.Description, updatedRuleName.Description);
            Assert.Equal(ruleNameViewModel.Json, updatedRuleName.Json);
            Assert.Equal(ruleNameViewModel.Template, updatedRuleName.Template);
            Assert.Equal(ruleNameViewModel.SqlStr, updatedRuleName.SqlStr);
            Assert.Equal(ruleNameViewModel.SqlPart, updatedRuleName.SqlPart);
        }

        [Fact]
        public async Task UpdateRuleName_ShouldReturnNotFound_WhenRuleNameDoesNotExist()
        {
            // Arrange
            int id = 1;
            _ruleNameServiceMock.Setup(s => s.FindByIdAsync(id)).ReturnsAsync((RuleName)null);

            // Act
            var result = await _controller.UpdateRuleName(id, new RuleNameViewModel());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UpdateRuleName_ShouldReturnBadRequest_WhenDataIsInvalid()
        {
            // Arrange
            int id = 0;

            // Act
            var result = await _controller.UpdateRuleName(id, null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteRuleName_ShouldReturnOk_WhenRuleNameIsDeleted()
        {
            // Arrange
            int id = 1;
            var existingRuleName = new RuleName { Id = id };

            _ruleNameServiceMock.Setup(s => s.FindByIdAsync(id)).ReturnsAsync(existingRuleName);
            _ruleNameServiceMock.Setup(s => s.DeleteAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteRuleName(id);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteRuleName_ShouldReturnNotFound_WhenRuleNameIsNotFound()
        {
            // Arrange
            int id = 1;
            _ruleNameServiceMock.Setup(s => s.FindByIdAsync(id)).ReturnsAsync((RuleName)null);

            // Act
            var result = await _controller.DeleteRuleName(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion
    }
}
