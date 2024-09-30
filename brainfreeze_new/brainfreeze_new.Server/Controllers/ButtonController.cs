using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace brainfreeze_new.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ButtonController : ControllerBase
    {
        [HttpPost]
        public IActionResult ReceiveButtonPress([FromBody] ButtonData data)
        {
            Console.WriteLine($"Button {data.ButtonId} pressed at {data.Timestamp}");
            return Ok(data);
        }
    }
}
