using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace brainfreeze_new.Server.Controllers.Tests 
{
    public class CardFlipControllerTests
    {
        private readonly Mock<ILogger<CardFlipController>> _mockLogger;
        private readonly CardFlipController _controller;

        public CardFlipControllerTests()
        {
            _mockLogger = new Mock<ILogger<CardFlipController>>();
            _controller = new CardFlipController(_mockLogger.Object);
        }

        [Fact]
        public void GetShuffledImages_Returns_ShuffledImageList()
        {
            
            var result = _controller.GetShuffledImages() as OkObjectResult;
            
            
            Assert.NotNull(result);
            
            
            var value = result.Value;
            var shuffledImagesProperty = value.GetType().GetProperty("shuffledImages");
            Assert.NotNull(shuffledImagesProperty);
            
            var shuffledImages = shuffledImagesProperty.GetValue(value) as List<string>;
            
            Assert.NotNull(shuffledImages);
            Assert.Equal(18, shuffledImages.Count); 
        }

        [Fact]
        public void GetHighScore_Returns_DefaultHighScore()
        {
            
            var result = _controller.GetHighScore() as OkObjectResult;
            
           
            Assert.NotNull(result);
            
            
            var value = result.Value;
            var highScoreProperty = value.GetType().GetProperty("highScore");
            Assert.NotNull(highScoreProperty);
            
            var highScore = (int)highScoreProperty.GetValue(value);
            Assert.Equal(0, highScore); 
        }

        [Fact]
        public void SubmitScore_Adds_NewHighScore()
        {
            var scoreData = new ScoreData { Score = 10 };

            var result = _controller.SubmitScore(scoreData) as OkObjectResult;
          
            Assert.NotNull(result);
            
            
            var value = result.Value;
            var newHighScoreProperty = value.GetType().GetProperty("newHighScore");
            Assert.NotNull(newHighScoreProperty);
            
            var isNewHighScore = (bool)newHighScoreProperty.GetValue(value);
            Assert.True(isNewHighScore);
        }

        [Fact]
        public void SubmitScore_Rejects_HigherScores()
        {
            
            _controller.SubmitScore(new ScoreData { Score = 10 }); 
            var scoreData = new ScoreData { Score = 15 };

           
            var result = _controller.SubmitScore(scoreData) as OkObjectResult;
            
            
            Assert.NotNull(result);
            
            var value = result.Value;
            var newHighScoreProperty = value.GetType().GetProperty("newHighScore");
            Assert.NotNull(newHighScoreProperty);
            
            var isNewHighScore = (bool)newHighScoreProperty.GetValue(value);
            Assert.False(isNewHighScore); 
        }
    }
    
}