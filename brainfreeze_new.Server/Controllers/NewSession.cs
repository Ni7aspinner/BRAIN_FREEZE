using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]

// Simple))). Creates a session id its an API endpoint https://localhost:7005/api/session/new
public class SessionController : ControllerBase
{
    private static Dictionary<string, string> sessions = new Dictionary<string, string>();

    [HttpGet("new")]
    public IActionResult GetNewSessionId()
    {
        var sessionId = Guid.NewGuid().ToString();
        sessions[sessionId] = sessionId;

        return Ok(new { sessionId });
    }
}
