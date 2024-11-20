using Microsoft.AspNetCore.Mvc;

namespace brainfreeze_new.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardFlipController : ControllerBase
    {
        private readonly ILogger<CardFlipController> _logger;
        private static int highScore = 0; 

        public CardFlipController(ILogger<CardFlipController> logger)
        {
            _logger = logger;
        }

        [HttpPost("submitInitScore")]
        public ActionResult SubmitInitScore([FromBody] ScoreData scoreData)
        {
            highScore = scoreData.Score;
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
            return Ok(new { highScore });
        }

        
        [HttpPost("submitScore")]
        public ActionResult SubmitScore([FromBody] ScoreData scoreData)
        {
            if (highScore == 0 || scoreData.Score < highScore) // If high score is 0 or the new score is better
            {
                highScore = scoreData.Score;
                _logger.LogInformation($"New high score: {highScore}");
                return Ok(new { newHighScore = true });
            }

            _logger.LogInformation($"Score submitted: {scoreData.Score}");
            return Ok(new { newHighScore = false });
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
