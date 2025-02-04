using AutoMapper;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;
using P7CreateRestApi.Data.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace P7CreateRestApi.Tests
{
    public class BidListControllerTests
    {
        private readonly Mock<IBidListService> _bidListServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BidListController _controller;

        public BidListControllerTests()
        {
            _bidListServiceMock = new Mock<IBidListService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new BidListController(_bidListServiceMock.Object, _mapperMock.Object);
        }


        #region ----- Create Tests -----
        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenBidListIsCreated()
        {
            // Arrange
            var bidListViewModel = new BidListViewModel { Account = "Account1",  BidQuantity = 10 };
            var bidList = new BidList { BidListId = 1, Account = "Account1",  BidQuantity = 10 };

            _mapperMock.Setup(m => m.Map<BidList>(bidListViewModel)).Returns(bidList);
            _bidListServiceMock.Setup(s => s.AddAsync(bidList)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(bidListViewModel);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.Create), createdResult.ActionName);
            Assert.Equal(bidList.BidListId, ((BidList)createdResult.Value).BidListId);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Account", "Required");

            // Act
            var result = await _controller.Create(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        #endregion

        #region ----- Read Tests -----
        [Fact]
        public async Task ReadEntity_ShouldReturnOk_WhenBidListIsFound()
        {
            // Arrange
            int bidListId = 1;
            var bidList = new BidList { BidListId = bidListId, Account = "Account1",  BidQuantity = 10 };
            var bidListViewModel = new BidListViewModel { Account = "Account1",  BidQuantity = 10 };

            _bidListServiceMock.Setup(s => s.FindByIdAsync(bidListId)).ReturnsAsync(bidList);
            _mapperMock.Setup(m => m.Map<BidListViewModel>(bidList)).Returns(bidListViewModel);

            // Act
            var result = await _controller.ReadEntity(bidListId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(bidListViewModel, okResult.Value);
        }

        [Fact]
        public async Task ReadEntity_ShouldReturnNotFound_WhenBidListIsNotFound()
        {
            // Arrange
            int bidListId = 1;
            _bidListServiceMock.Setup(s => s.FindByIdAsync(bidListId)).ReturnsAsync((BidList)null);

            // Act
            var result = await _controller.ReadEntity(bidListId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
        #endregion

        #region ----- Update Tests -----

        [Fact]
        public async Task UpdateBid_ShouldReturnOk_WhenBidListIsUpdated()
        {
            // Arrange
            int bidListId = 1;
            var bidListViewModel = new BidListViewModel { Account = "UpdatedAccount",  BidQuantity = 20 };
            var existingBid = new BidList { BidListId = bidListId, Account = "OldAccount",  BidQuantity = 10 };

            _bidListServiceMock.Setup(s => s.FindByIdAsync(bidListId)).ReturnsAsync(existingBid);
            _mapperMock.Setup(m => m.Map(bidListViewModel, existingBid)).Returns(existingBid);
            _bidListServiceMock.Setup(s => s.UpdateAsync(existingBid)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<BidListViewModel>(existingBid)).Returns(bidListViewModel);

            // Act
            var result = await _controller.UpdateBid(bidListId, bidListViewModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(bidListViewModel, okResult.Value);
        }

        [Fact]
        public async Task UpdateBid_ShouldReturnNotFound_WhenBidListIsNotFound()
        {
            // Arrange
            int bidListId = 1;
            _bidListServiceMock.Setup(s => s.FindByIdAsync(bidListId)).ReturnsAsync((BidList)null);

            // Act
            var result = await _controller.UpdateBid(bidListId, new BidListViewModel());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }


        #endregion

        #region ----- Delete Tests -----

        [Fact]
        public async Task DeleteBid_ShouldReturnNoContent_WhenBidListIsDeleted()
        {
            // Arrange
            int bidListId = 1;
            var existingBid = new BidList { BidListId = bidListId };

            _bidListServiceMock.Setup(s => s.FindByIdAsync(bidListId)).ReturnsAsync(existingBid);
            _bidListServiceMock.Setup(s => s.DeleteAsync(bidListId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteBid(bidListId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteBid_ShouldReturnNotFound_WhenBidListIsNotFound()
        {
            // Arrange
            int bidListId = 1;
            _bidListServiceMock.Setup(s => s.FindByIdAsync(bidListId)).ReturnsAsync((BidList)null);

            // Act
            var result = await _controller.DeleteBid(bidListId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }


        #endregion
    }
}
