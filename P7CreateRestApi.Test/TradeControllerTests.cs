using AutoMapper;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;
using P7CreateRestApi.Data.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace P7CreateRestApi.Tests
{
    public class TradeControllerTests
    {
        private readonly Mock<ITradeService> _mockTradeService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly TradeController _controller;

        public TradeControllerTests()
        {
            _mockTradeService = new Mock<ITradeService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new TradeController(_mockTradeService.Object, _mockMapper.Object);
        }

        #region Create Trade
        [Fact]
        public async Task CreateTrade_ShouldReturnCreatedAtAction_WhenValid()
        {
            // Arrange
            var tradeViewModel = new TradeViewModel { Account = "TestAccount" };
            var trade = new Trade { TradeId = 1, Account = "TestAccount" };

            _mockMapper.Setup(m => m.Map<Trade>(tradeViewModel)).Returns(trade);
            _mockTradeService.Setup(s => s.AddTradeAsync(trade)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateTrade(tradeViewModel);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(trade.TradeId, ((Trade)createdResult.Value).TradeId);
        }
        #endregion

        #region Get Trade
        [Fact]
        public async Task GetTrade_ShouldReturnOk_WhenTradeExists()
        {
            // Arrange
            var trade = new Trade { TradeId = 1, Account = "TestAccount" };
            _mockTradeService.Setup(s => s.GetTradeByIdAsync(1)).ReturnsAsync(trade);
            _mockMapper.Setup(m => m.Map<TradeViewModel>(trade)).Returns(new TradeViewModel { Account = "TestAccount" });

            // Act
            var result = await _controller.GetTrade(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task GetTrade_ShouldReturnNotFound_WhenTradeDoesNotExist()
        {
            // Arrange
            _mockTradeService.Setup(s => s.GetTradeByIdAsync(1)).ReturnsAsync((Trade)null);

            // Act
            var result = await _controller.GetTrade(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
        #endregion

        #region Update Trade
        [Fact]
        public async Task UpdateTrade_ShouldReturnOk_WhenTradeIsUpdated()
        {
            // Arrange
            var tradeViewModel = new TradeViewModel { Account = "UpdatedAccount" };
            var existingTrade = new Trade { TradeId = 1, Account = "OldAccount" };

            _mockTradeService.Setup(s => s.GetTradeByIdAsync(1)).ReturnsAsync(existingTrade);
            _mockMapper.Setup(m => m.Map(tradeViewModel, existingTrade));
            _mockTradeService.Setup(s => s.UpdateTradeAsync(existingTrade)).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<TradeViewModel>(existingTrade)).Returns(tradeViewModel);

            // Act
            var result = await _controller.UpdateTrade(1, tradeViewModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task UpdateTrade_ShouldReturnNotFound_WhenTradeDoesNotExist()
        {
            // Arrange
            _mockTradeService.Setup(s => s.GetTradeByIdAsync(1)).ReturnsAsync((Trade)null);
            var tradeViewModel = new TradeViewModel { Account = "UpdatedAccount" };

            // Act
            var result = await _controller.UpdateTrade(1, tradeViewModel);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
        #endregion

        #region Delete Trade
        [Fact]
        public async Task DeleteTrade_ShouldReturnNoContent_WhenTradeIsDeleted()
        {
            // Arrange
            var existingTrade = new Trade { TradeId = 1 };
            _mockTradeService.Setup(s => s.GetTradeByIdAsync(1)).ReturnsAsync(existingTrade);
            _mockTradeService.Setup(s => s.DeleteTradeAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteTrade(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTrade_ShouldReturnNotFound_WhenTradeDoesNotExist()
        {
            // Arrange
            _mockTradeService.Setup(s => s.GetTradeByIdAsync(1)).ReturnsAsync((Trade)null);

            // Act
            var result = await _controller.DeleteTrade(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
        #endregion
    }
}
