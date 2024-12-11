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

        [Fact]
        public void EvaluateScore_DifficultyNotGiven()
        {
            var request = new ScoreEvaluationRequest
            {
                UserInput = new int[] { 1, 2, 7},
                Pattern = new int[] { 1, 2, 7},
            };

            var result = _controller.EvaluateScore(request) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var response = result.Value as ScoreResponse;
            Assert.NotNull(response);
            Assert.Equal(3, response.Score);
        }

        [Fact]
        public void EvaluateScore_VeryEasyDifficulty()
        {
            var request = new ScoreEvaluationRequest
            {
                UserInput = new int[] { 1, 2 },
                Pattern = new int[] { 1, 2 },
                Difficulty = DifficultyLevel.VeryEasy
            };

            var result = _controller.EvaluateScore(request) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var response = result.Value as ScoreResponse;
            Assert.NotNull(response);
            Assert.Equal(2, response.Score);
        }

        [Fact]
        public void EvaluateScore_EasyDifficulty()
        {
            var request = new ScoreEvaluationRequest
            {
                UserInput = new int[] { 4, 5, 6 },
                Pattern = new int[] { 4, 5, 6 },
                Difficulty = DifficultyLevel.Easy
            };

            var result = _controller.EvaluateScore(request) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var response = result.Value as ScoreResponse;
            Assert.NotNull(response);
            Assert.Equal(6, response.Score);
        }

        [Fact]
        public void EvaluateScore_NormalDifficulty()
        {
            var request = new ScoreEvaluationRequest
            {
                UserInput = new int[] { 7, 8, 9 },
                Pattern = new int[] { 7, 8, 9 },
                Difficulty = DifficultyLevel.Normal
            };

            var result = _controller.EvaluateScore(request) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var response = result.Value as ScoreResponse;
            Assert.NotNull(response);
            Assert.Equal(9, response.Score);
        }

        [Fact]
        public void EvaluateScore_HardDifficulty()
        {
            var request = new ScoreEvaluationRequest
            {
                UserInput = new int[] { 13, 14, 15 },
                Pattern = new int[] { 13, 14, 15 },
                Difficulty = DifficultyLevel.Hard
            };

            var result = _controller.EvaluateScore(request) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var response = result.Value as ScoreResponse;
            Assert.NotNull(response);
            Assert.Equal(12, response.Score);
        }

        [Fact]
        public void EvaluateScore_NightmareDifficulty()
        {
            var request = new ScoreEvaluationRequest
            {
                UserInput = new int[] { 16, 17, 18, 19 },
                Pattern = new int[] { 16, 17, 18, 19 },
                Difficulty = DifficultyLevel.Nightmare
            };

            var result = _controller.EvaluateScore(request) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var response = result.Value as ScoreResponse;
            Assert.NotNull(response);
            Assert.Equal(20, response.Score);
        }

        [Fact]
        public void EvaluateScore_ImpossibleDifficulty()
        {
            var request = new ScoreEvaluationRequest
            {
                UserInput = new int[] { 20, 21, 22 },
                Pattern = new int[] { 20, 21, 22 },
                Difficulty = DifficultyLevel.Impossible
            };

            var result = _controller.EvaluateScore(request) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var response = result.Value as ScoreResponse;
            Assert.NotNull(response);
            Assert.Equal(18, response.Score);
        }


    }
}