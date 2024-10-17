using Microsoft.AspNetCore.Mvc;
namespace brainfreeze_new.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoreController : ControllerBase {
        [HttpPost("evaluate")]
        public IActionResult EvaluateScore([FromBody] ScoreEvaluationRequest request)
        {
            int score = CalculateScore(request.UserInput, request.Pattern, request.Difficulty);
            return Ok(new { score });
        }

        private int CalculateScore(int[] userInput, int[] pattern, DifficultyLevel difficulty)
        {
            int baseScore = 0;

            // Increment score if the user input matches the pattern
            for (int i = 0; i < pattern.Length; i++)
            {
                if (i < userInput.Length && userInput[i] == pattern[i])
                {
                    baseScore++;
                }
                else
                {
                    break; // End the scoring as soon as the pattern doesn't match
                }
            }

            // Adjust the score based on difficulty
            int difficultyMultiplier = difficulty switch
            {
                DifficultyLevel.VeryEasy => 1,
                DifficultyLevel.Easy => 2,
                DifficultyLevel.Normal => 3,
                DifficultyLevel.Hard => 4,
                DifficultyLevel.Nightmare => 5,
                DifficultyLevel.Impossible => 6,
                _ => 1
            };

            return baseScore * difficultyMultiplier;
        }
    }
    public class ScoreEvaluationRequest
    {
        public required int[] UserInput { get; set; }
        public required int[] Pattern { get; set; }
        public DifficultyLevel Difficulty { get; set; }
    }
}