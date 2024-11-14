using Microsoft.AspNetCore.Mvc;
using brainfreeze_new.Server.Controllers;
using Xunit;

namespace brainfreeze_new.Tests.Controllers
{
    public class ScoreControllerTests
    {
        private readonly ScoreController _controller;

        public ScoreControllerTests()
        {
            _controller = new ScoreController();
        }

        [Fact]
        public void EvaluateScore_ExactPatternMatch()
        {
            var request = new ScoreEvaluationRequest
            {
                UserInput = new int[] { 1, 2, 3 },
                Pattern = new int[] { 1, 2, 3 },
                Difficulty = DifficultyLevel.Normal
            };

            var result = _controller.EvaluateScore(request) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var response = result.Value as ScoreResponse;
            Assert.NotNull(response);
            // 3 matches * Normal multiplier (3) = 9
            Assert.Equal(9, response.Score);
        }

        [Fact]
        public void EvaluateScore_NoMatch()
        {
            var request = new ScoreEvaluationRequest
            {
                UserInput = new int[] { 4, 5, 6 },
                Pattern = new int[] { 1, 2, 3 },
                Difficulty = DifficultyLevel.Easy
            };

            var result = _controller.EvaluateScore(request) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var response = result.Value as ScoreResponse;
            Assert.NotNull(response);
            // No match => score is 0
            Assert.Equal(0, response.Score);
        }

        [Fact]
        public void EvaluateScore_NoMatchDifferentLength()
        {
            var request = new ScoreEvaluationRequest
            {
                UserInput = new int[] { 7 },
                Pattern = new int[] { 1, 2, 3, 4 },
                Difficulty = DifficultyLevel.Hard
            };

            var result = _controller.EvaluateScore(request) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var response = result.Value as ScoreResponse;
            Assert.NotNull(response);
            // No match => score is 0
            Assert.Equal(0, response.Score);
        }
    }
}