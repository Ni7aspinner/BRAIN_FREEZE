using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace brainfreeze_new.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardFlipController : ControllerBase
    {
        private readonly ILogger<CardFlipController> _logger;
        private static readonly ConcurrentDictionary<string, int> HighScores = new ConcurrentDictionary<string, int>();

        public CardFlipController(ILogger<CardFlipController> logger)
        {
            _logger = logger;
        }

        [HttpPost("submitInitScore")]
        public ActionResult SubmitInitScore([FromBody] ScoreData scoreData)
        {
            int highScore = scoreData.Score;
            return Ok(new { highScore });
        }

        [HttpGet("shuffledImages")]
        public ActionResult GetShuffledImages()
        {
            var shuffledImages = GetShuffledImageList();
            return Ok(new { shuffledImages });
        }

        [HttpGet("highscore")]
        public ActionResult GetHighScore()
        {
            HighScores.TryGetValue("globalHighScore", out int highScore);
            return Ok(new { highScore });
        }

        [HttpPost("submitScore")]
        public ActionResult SubmitScore([FromBody] ScoreData scoreData)
        {
            HighScores.AddOrUpdate("globalHighScore",
                scoreData.Score, 
                (key, currentHighScore) => scoreData.Score < currentHighScore ? scoreData.Score : currentHighScore);

            HighScores.TryGetValue("globalHighScore", out int newHighScore);
            bool isNewHighScore = newHighScore == scoreData.Score;

            _logger.LogInformation(isNewHighScore
                ? $"New high score: {newHighScore}"
                : $"Score submitted: {scoreData.Score}");

            return Ok(new { newHighScore = isNewHighScore });
        }

        private List<string> GetShuffledImageList()
        {
            var images = new List<string>
            {
                "src/assets/bf.png", "src/assets/gb.png", "src/assets/icecube.png", "src/assets/think.png", 
                "src/assets/skate.png", "src/assets/sleep.png", "src/assets/bigbrain.png", "src/assets/gear.png", 
                "src/assets/icecream.png"
            };

            var allImages = images.Concat(images).ToList();

            Random rand = new Random();
            for (int i = allImages.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                var temp = allImages[i];
                allImages[i] = allImages[j];
                allImages[j] = temp;
            }

            return allImages;
        }
    }

    public class ScoreData
    {
        public int Score { get; set; }
    }
}
