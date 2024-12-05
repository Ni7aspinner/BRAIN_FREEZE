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
            _mockLogger = new Mock<ILogger<DataController>>();
            _controller = new DataController(_mockLogger.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkWithDefaultLevel()
        {
            var level = DifficultyLevel.VeryEasy;

            var result = await _controller.Get(level);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var responseData = Assert.IsType<DataController.ResponseData>(okResult.Value);

            Assert.NotNull(responseData.Data);
            Assert.Equal("Game Started!", responseData.Message);
        }

        [Fact]
        public async Task Get_CustomLevel_ThrowsExceptionForMissingFile()
        {
            var level = DifficultyLevel.Custom;

            await Assert.ThrowsAnyAsync<System.IO.IOException>(() => _controller.Get(level));
        }

        [Fact]
        public async Task Add_ValidData_ReturnsCongrats()
        {
            var jsonData = @"{
                ""createdList"": [1, 2, 3],
                ""expectedList"": [1, 2, 3],
                ""difficulty"": 3
            }";

            var data = JsonSerializer.Deserialize<Data>(jsonData);

            Assert.NotNull(data);

            var result = await _controller.Add(data);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var responseData = Assert.IsType<DataController.ResponseData>(okResult.Value);

            Assert.Equal("Congrats player!", responseData.Message);
            Assert.NotNull(responseData.Data);
        }

        [Fact]
        public async Task Add_InvalidData_ReturnsBadRequest()
        {
            var data = new Data
            {
                CreatedList = null,
                ExpectedList = null
            };
            var result = await _controller.Add(data);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid data", badRequestResult.Value);
        }

        [Fact]
        public async Task Add_IncorrectData_ReturnsNewSequence()
        {
            var data = new Data
            {
                CreatedList = new List<object> { 1, 2, 3 },
                ExpectedList = new List<object> { 1, 2, 4 },
                Difficulty = 3
            };

            var result = await _controller.Add(data);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var responseData = Assert.IsType<DataController.ResponseData>(okResult.Value);

            Assert.Equal("Loser!", responseData.Message);
            Assert.NotNull(responseData.Data);
        }


        [Fact]
        public async Task Get_CustomLevel_Executes_CustomList()
        {
            var data = new Data();
            string tempFileName = Path.GetTempFileName();
            await File.WriteAllTextAsync(tempFileName, "1, 2, 3, 4, 5"); 
            File.Copy(tempFileName, "Challenge.txt", overwrite: true);

            var result = await _controller.Get(DifficultyLevel.Custom);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var responseData = Assert.IsType<DataController.ResponseData>(okResult.Value);

            Assert.NotNull(responseData.Data);
            Assert.Equal(new object[] { 1, 2, 3, 4, 5 }, responseData.Data.CreatedList);

            File.Delete(tempFileName);
            File.Delete("Challenge.txt");
        }


        // ShortCheck tests

        [Fact]
        public void ShortCheck_ReturnsFalse_WhenCreatedListIsNull()
        {
            var data = new Data
            {
                CreatedList = null,
                ExpectedList = new List<object> { 1, 2, 3 }
            };

            var result = DataController.ShortCheck(data);
            Assert.False(result);
        }

        [Fact]
        public void ShortCheck_ReturnsFalse_WhenExpectedListIsNull()
        {
            var data = new Data
            {
                CreatedList = new List<object> { 1, 2, 3 },
                ExpectedList = null
            };

            var result = DataController.ShortCheck(data);

            Assert.False(result);
        }

        [Fact]
        public void ShortCheck_ReturnsFalse_WhenCreatedListIsShorterThanExpectedList()
        {
            var data = new Data
            {
                CreatedList = new List<object> { 1 },
                ExpectedList = new List<object> { 1, 2 }
            };

            var result = DataController.ShortCheck(data);

            Assert.False(result);
        }

        [Fact]
        public void ShortCheck_ReturnsTrue_WhenCreatedListIsEqualToExpectedList()
        {
            var data = new Data
            {
                CreatedList = new List<object> { 1, 2, 3 },
                ExpectedList = new List<object> { 1, 2, 3 }
            };

            var result = DataController.ShortCheck(data);

            Assert.True(result);
        }

        [Fact]
        public void ShortCheck_ReturnsTrue_WhenCreatedListIsLongerThanExpectedList()
        {
            var data = new Data
            {
                CreatedList = new List<object> { 1, 2, 3, 4, 5 },
                ExpectedList = new List<object> { 1, 2, 3 }
            };
            var result = DataController.ShortCheck(data);

            Assert.True(result);
        }

        //Check tests

        [Fact]
        public void Check_AreEqual_ReturnsTrue_WhenListsAreEqual()
        {
            var data = new Data
            {
                CreatedList = new List<object> { 1, 2, 3 },
                ExpectedList = new List<object> { 1, 2, 3 }
            };

            var check = new DataController.Check(data);


            Assert.True(check.AreEqual);
        }

        [Fact]
        public void Check_AreEqual_ReturnsFalse_WhenListsAreNotEqual()
        {
            var data = new Data
            {
                CreatedList = new List<object> { 1, 2, 4 },
                ExpectedList = new List<object> { 1, 2, 3 }
            };

            var check = new DataController.Check(data);

            Assert.False(check.AreEqual);
        }

        [Fact]
        public void Check_AreEqual_ReturnsFalse_WhenCreatedListHasExtraElements()
        {
            var data = new Data
            {
                CreatedList = new List<object> { 1, 2, 3, 4 },
                ExpectedList = new List<object> { 1, 2, 3 }
            };

            var check = new DataController.Check(data);

            Assert.False(check.AreEqual);
        }

        [Fact]
        public void Check_AreEqual_ReturnsFalse_WhenCreatedListIsEmpty()
        {
            var data = new Data
            {
                CreatedList = new List<object>(),
                ExpectedList = new List<object> { 1, 2, 3 }
            };

            var check = new DataController.Check(data);

            Assert.False(check.AreEqual);
        }

        [Fact]
        public void Check_AreEqual_ReturnsFalse_WhenExpectedListIsEmpty()
        {
            var data = new Data
            {
                CreatedList = new List<object> { 1, 2, 3 },
                ExpectedList = new List<object>()
            };
            var check = new DataController.Check(data);

            Assert.False(check.AreEqual);
        }

        [Fact]
        public void Check_AreEqual_ReturnsTrue_WithJsonElementLists()
        {
            var jsonArray = JsonSerializer.SerializeToElement(new List<int> { 1, 2, 3 });
            var data = new Data
            {
                CreatedList = jsonArray.EnumerateArray().Select(item => (object)item).ToList(),
                ExpectedList = new List<object> { 1, 2, 3 }
            };

            var check = new DataController.Check(data);

            Assert.True(check.AreEqual);
        }
    }
}
