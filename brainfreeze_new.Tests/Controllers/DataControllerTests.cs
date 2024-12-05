using brainfreeze_new.Server;
using brainfreeze_new.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace brainfreeze_new.Tests.Controllers
{
    public class DataControllerTests
    {
        private readonly Mock<ILogger<DataController>> _mockLogger;
        private readonly DataController _controller;

        public DataControllerTests()
        {
            // Create a mocked ILogger
            _mockLogger = new Mock<ILogger<DataController>>();
            _controller = new DataController(_mockLogger.Object);
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

        [Fact]
        public async Task Get_CustomLevel_Executes_CustomList()
        {
            // Arrange
            var data = new Data();
            string tempFileName = Path.GetTempFileName();
            await File.WriteAllTextAsync(tempFileName, "1, 2, 3, 4, 5");

            // Ensure "Challenge.txt" is accessible
            File.Copy(tempFileName, "Challenge.txt", overwrite: true);

            // Act
            var result = await _controller.Get(DifficultyLevel.Custom);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var responseData = Assert.IsType<DataController.ResponseData>(okResult.Value);

            Assert.NotNull(responseData.Data);
            Assert.Equal(new object[] { 1, 2, 3, 4, 5 }, responseData.Data.CreatedList);

            // Clean up
            File.Delete(tempFileName);
            File.Delete("Challenge.txt");
        }
    }
}
