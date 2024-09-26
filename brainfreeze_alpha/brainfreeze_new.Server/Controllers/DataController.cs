using Microsoft.AspNetCore.Mvc;

namespace brainfreeze_new.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IncController : ControllerBase
    {

        private readonly ILogger<IncController> _logger;

        public IncController(ILogger<IncController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetData")]
        public IEnumerable<Data> Get()
        {
            return Enumerable.Range(1, 12).Select(index => new Data
            {
                number = Random.Shared.Next(1, 9)
            })
            .ToArray();
        }
    }
}
