using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using brainfreeze_new.Server.Controllers;
using Xunit;
using System.Collections.Generic;
using brainfreeze_new.Server;
using Microsoft.Extensions.Logging.Abstractions;

namespace brainfreeze_new.Tests.Controllers
{
    public class NRGControllerTests
    {
        private readonly NRGController _controller;

        public NRGControllerTests()
        {
            _controller = new NRGController(NullLogger<NRGController>.Instance);
        }

        [Fact]
        public void Get_ReturnsOkResult_WithValidData()
        {
            var result = _controller.Get(DifficultyLevel.VeryEasy);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var responseData = Assert.IsType<NRGController.ResponseData>(okResult.Value);

            Assert.NotNull(responseData);
            Assert.NotEmpty(responseData.data.CreatedList);
            Assert.Equal("Game started!", responseData.Message);
        }

        [Fact]
        public void Add_ReturnsOkResult_WhenSequenceIsEqual()
        {
            var sequence = new GenericClass<int>
            {
                CreatedList = new List<int> { 1, 2, 3 },
                ExpectedList = new List<int> { 1, 2, 3 }
            };

            var result = _controller.Add(sequence);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var responseData = Assert.IsType<NRGController.ResponseData>(okResult.Value);
            Assert.Equal("Congrats player!", responseData.Message);
            Assert.Equal(sequence.CreatedList.Count, responseData.data.CreatedList.Count);
        }

        [Fact]
        public void Add_ReturnsOkResult_WhenSequenceIsNotEqual()
        {
            var sequence = new GenericClass<int>
            {
                CreatedList = new List<int> { 1, 2, 3 },
                ExpectedList = new List<int> { 4, 5, 6 }
            };

            var result = _controller.Add(sequence);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var responseData = Assert.IsType<NRGController.ResponseData>(okResult.Value);
            Assert.Equal("Loser!", responseData.Message);
            Assert.NotEqual(sequence.CreatedList.Count, responseData.data.CreatedList.Count);
        }

    }
}
