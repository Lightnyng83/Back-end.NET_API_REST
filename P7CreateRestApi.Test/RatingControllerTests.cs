using AutoMapper;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Controllers.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;
using P7CreateRestApi.Data.Services;
using System.Threading.Tasks;
using Xunit;

namespace P7CreateRestApi.Tests
{
    public class RatingControllerTests
    {
        private readonly Mock<IRatingService> _ratingServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly RatingController _controller;

        public RatingControllerTests()
        {
            _ratingServiceMock = new Mock<IRatingService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new RatingController(_ratingServiceMock.Object, _mapperMock.Object);
        }

        #region Create

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenRatingIsCreated()
        {
            // Arrange
            var ratingViewModel = new RatingViewModel
            {
                MoodysRating = "AAA",
                SandPRating = "AA+",
                FitchRating = "A",
                OrderNumber = 1
            };
            var rating = new Rating
            {
                Id = 1,
                MoodysRating = "AAA",
                SandPRating = "AA+",
                FitchRating = "A",
                OrderNumber = 1
            };

            _mapperMock.Setup(m => m.Map<Rating>(ratingViewModel)).Returns(rating);
            _ratingServiceMock.Setup(s => s.AddAsync(rating)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(ratingViewModel);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.Create), createdResult.ActionName);
            Assert.Equal(rating.Id, ((Rating)createdResult.Value).Id);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("MoodysRating", "Required");

            // Act
            var result = await _controller.Create(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        #endregion

        #region Read

        [Fact]
        public async Task GetEntity_ShouldReturnOk_WhenRatingIsFound()
        {
            // Arrange
            int ratingId = 1;
            var rating = new Rating
            {
                Id = ratingId,
                MoodysRating = "AAA",
                SandPRating = "AA+",
                FitchRating = "A",
                OrderNumber = 1
            };
            var ratingViewModel = new RatingViewModel
            {
                MoodysRating = "AAA",
                SandPRating = "AA+",
                FitchRating = "A",
                OrderNumber = 1
            };

            _ratingServiceMock.Setup(s => s.FindByIdAsync(ratingId)).ReturnsAsync(rating);
            _mapperMock.Setup(m => m.Map<RatingViewModel>(rating)).Returns(ratingViewModel);

            // Act
            var result = await _controller.GetEntity(ratingId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(rating, okResult.Value);
        }

        [Fact]
        public async Task GetEntity_ShouldReturnNotFound_WhenRatingIsNotFound()
        {
            // Arrange
            int ratingId = 1;
            _ratingServiceMock.Setup(s => s.FindByIdAsync(ratingId)).ReturnsAsync((Rating)null);

            // Act
            var result = await _controller.GetEntity(ratingId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateRating_ShouldReturnOk_WhenRatingIsUpdated()
        {
            // Arrange
            int ratingId = 1;
            var ratingViewModel = new RatingViewModel
            {
                MoodysRating = "BBB",
                SandPRating = "BB+",
                FitchRating = "BB",
                OrderNumber = 2
            };
            var existingRating = new Rating
            {
                Id = ratingId,
                MoodysRating = "AAA",
                SandPRating = "AA+",
                FitchRating = "A",
                OrderNumber = 1
            };

            _ratingServiceMock.Setup(s => s.FindByIdAsync(ratingId)).ReturnsAsync(existingRating);
            _mapperMock.Setup(m => m.Map(ratingViewModel, existingRating)).Returns(existingRating);
            _ratingServiceMock.Setup(s => s.UpdateAsync(existingRating)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<RatingViewModel>(existingRating)).Returns(ratingViewModel);

            // Act
            var result = await _controller.UpdateRating(ratingId, ratingViewModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(ratingViewModel, okResult.Value);
        }

        [Fact]
        public async Task UpdateRating_ShouldReturnNotFound_WhenRatingIsNotFound()
        {
            // Arrange
            int ratingId = 1;
            _ratingServiceMock.Setup(s => s.FindByIdAsync(ratingId)).ReturnsAsync((Rating)null);

            // Act
            var result = await _controller.UpdateRating(ratingId, new RatingViewModel());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UpdateRating_ShouldReturnBadRequest_WhenDataIsInvalid()
        {
            // Arrange
            int ratingId = 0;

            // Act
            var result = await _controller.UpdateRating(ratingId, null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteRating_ShouldReturnNoContent_WhenRatingIsDeleted()
        {
            // Arrange
            int ratingId = 1;
            var existingRating = new Rating { Id = ratingId };

            _ratingServiceMock.Setup(s => s.FindByIdAsync(ratingId)).ReturnsAsync(existingRating);
            _ratingServiceMock.Setup(s => s.DeleteAsync(ratingId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteRating(ratingId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteRating_ShouldReturnNotFound_WhenRatingIsNotFound()
        {
            // Arrange
            int ratingId = 1;
            _ratingServiceMock.Setup(s => s.FindByIdAsync(ratingId)).ReturnsAsync((Rating)null);

            // Act
            var result = await _controller.DeleteRating(ratingId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion
    }
}
