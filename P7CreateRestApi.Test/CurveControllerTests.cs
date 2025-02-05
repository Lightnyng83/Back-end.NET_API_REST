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
    public class CurveControllerTests
    {
        private readonly Mock<ICurvePointService> _curvePointServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CurveController _controller;

        public CurveControllerTests()
        {
            _curvePointServiceMock = new Mock<ICurvePointService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new CurveController(_curvePointServiceMock.Object, _mapperMock.Object);
        }

        #region Create

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenCurvePointIsCreated()
        {
            // Arrange
            var curvePointViewModel = new CurvePointViewModel { Term = 2.0, CurvePointValue = 10.5 };
            var curvePoint = new CurvePoint { Id = 1, Term = 2.0, CurvePointValue = 10.5 };

            _mapperMock.Setup(m => m.Map<CurvePoint>(curvePointViewModel)).Returns(curvePoint);
            _curvePointServiceMock.Setup(s => s.AddAsync(curvePoint)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(curvePointViewModel);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.Create), createdResult.ActionName);
            Assert.Equal(curvePoint.Id, ((CurvePoint)createdResult.Value).Id);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Term", "Required");

            // Act
            var result = await _controller.Create(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        #endregion

        #region Read

        [Fact]
        public async Task GetEntity_ShouldReturnOk_WhenCurvePointIsFound()
        {
            // Arrange
            int curvePointId = 1;
            var curvePoint = new CurvePoint { Id = curvePointId, Term = 2.0, CurvePointValue = 10.5 };
            var curvePointViewModel = new CurvePointViewModel { Term = 2.0, CurvePointValue = 10.5 };

            _curvePointServiceMock.Setup(s => s.FindByIdAsync(curvePointId)).ReturnsAsync(curvePoint);
            _mapperMock.Setup(m => m.Map<CurvePointViewModel>(curvePoint)).Returns(curvePointViewModel);

            // Act
            var result = await _controller.GetEntity(curvePointId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(curvePointViewModel, okResult.Value);
        }

        [Fact]
        public async Task GetEntity_ShouldReturnNotFound_WhenCurvePointIsNotFound()
        {
            // Arrange
            int curvePointId = 1;
            _curvePointServiceMock.Setup(s => s.FindByIdAsync(curvePointId)).ReturnsAsync((CurvePoint)null);

            // Act
            var result = await _controller.GetEntity(curvePointId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateCurvePoint_ShouldReturnOk_WhenCurvePointIsUpdated()
        {
            // Arrange
            int curvePointId = 1;
            var curvePointViewModel = new CurvePointViewModel { Term = 3.0, CurvePointValue = 15.0 };
            var existingCurve = new CurvePoint { Id = curvePointId, Term = 2.0, CurvePointValue = 10.5 };

            _curvePointServiceMock.Setup(s => s.FindByIdAsync(curvePointId)).ReturnsAsync(existingCurve);
            _mapperMock.Setup(m => m.Map(curvePointViewModel, existingCurve)).Returns(existingCurve);
            _curvePointServiceMock.Setup(s => s.UpdateAsync(existingCurve)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<CurvePointViewModel>(existingCurve)).Returns(curvePointViewModel);

            // Act
            var result = await _controller.UpdateCurvePoint(curvePointId, curvePointViewModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(curvePointViewModel, okResult.Value);
        }

        [Fact]
        public async Task UpdateCurvePoint_ShouldReturnNotFound_WhenCurvePointIsNotFound()
        {
            // Arrange
            int curvePointId = 1;
            _curvePointServiceMock.Setup(s => s.FindByIdAsync(curvePointId)).ReturnsAsync((CurvePoint)null);

            // Act
            var result = await _controller.UpdateCurvePoint(curvePointId, new CurvePointViewModel());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UpdateCurvePoint_ShouldReturnBadRequest_WhenDataIsInvalid()
        {
            // Arrange
            int curvePointId = 0;

            // Act
            var result = await _controller.UpdateCurvePoint(curvePointId, null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteCurvePoint_ShouldReturnNoContent_WhenCurvePointIsDeleted()
        {
            // Arrange
            int curvePointId = 1;
            var existingCurve = new CurvePoint { Id = curvePointId };

            _curvePointServiceMock.Setup(s => s.FindByIdAsync(curvePointId)).ReturnsAsync(existingCurve);
            _curvePointServiceMock.Setup(s => s.DeleteAsync(curvePointId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteCurvePoint(curvePointId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCurvePoint_ShouldReturnNotFound_WhenCurvePointIsNotFound()
        {
            // Arrange
            int curvePointId = 1;
            _curvePointServiceMock.Setup(s => s.FindByIdAsync(curvePointId)).ReturnsAsync((CurvePoint)null);

            // Act
            var result = await _controller.DeleteCurvePoint(curvePointId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion
    }
}
