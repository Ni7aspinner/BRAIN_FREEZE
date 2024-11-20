using brainfreeze_new.Server.Controllers;
using brainfreeze_new.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Text.Json;

namespace brainfreeze_new.Tests.Controllers
{
    public class DataControllerTests
    {
        private readonly TestLogger<DataController> _testLogger;
        private readonly DataController _controller;

        public DataControllerTests()
        {
            // Use a custom test logger instead of a mocked ILogger
            _testLogger = new TestLogger<DataController>();
            _controller = new DataController(_testLogger);
        }

        [Fact]
        public async Task Get_ReturnsOkWithDefaultLevel()
        {
            // Arrange
            var level = DifficultyLevel.VeryEasy;

            // Act
            var result = await _controller.Get(level);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var responseData = Assert.IsType<DataController.ResponseData>(okResult.Value);

            Assert.NotNull(responseData.Data);
            Assert.Equal("Game Started!", responseData.Message);
        }

        [Fact]
        public async Task Get_CustomLevel_ThrowsExceptionForMissingFile()
        {
            // Arrange
            var level = DifficultyLevel.Custom;

            // Act & Assert
            // This will throw an exception because the `Challenge.txt` file does not exist
            await Assert.ThrowsAnyAsync<System.IO.IOException>(() => _controller.Get(level));
        }

        [Fact]
        public async Task Add_ValidData_ReturnsCongrats()
        {
            // Arrange
            var jsonData = @"{
                ""createdList"": [1, 2, 3],
                ""expectedList"": [1, 2, 3],
                ""difficulty"": 3
            }";

            var data = JsonSerializer.Deserialize<Data>(jsonData);

            Assert.NotNull(data);

            // Act
            var result = await _controller.Add(data);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var responseData = Assert.IsType<DataController.ResponseData>(okResult.Value);

            Assert.Equal("Congrats player!", responseData.Message);
            Assert.NotNull(responseData.Data);
        }

        [Fact]
        public async Task Add_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var data = new Data
            {
                CreatedList = null,
                ExpectedList = null
            };

            // Act
            var result = await _controller.Add(data);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid data", badRequestResult.Value);
        }

        [Fact]
        public async Task Add_IncorrectData_ReturnsNewSequence()
        {
            // Arrange
            var data = new Data
            {
                CreatedList = new List<object> { 1, 2, 3 },
                ExpectedList = new List<object> { 1, 2, 4 }, // Mismatch
                Difficulty = 4
            };

            // Act
            var result = await _controller.Add(data);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var responseData = Assert.IsType<DataController.ResponseData>(okResult.Value);

            Assert.Equal("Loser!", responseData.Message);
            Assert.NotNull(responseData.Data);
        }
    }

    // Custom test logger implementation
    public class TestLogger<T> : ILogger<T>
    {
        public List<string> Logs { get; } = new List<string>();

        IDisposable ILogger.BeginScope<TState>(TState state) => null!;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            ArgumentNullException.ThrowIfNull(formatter);

            // Add the formatted log message to the Logs collection
            Logs.Add(formatter(state, exception));
        }
    }
}