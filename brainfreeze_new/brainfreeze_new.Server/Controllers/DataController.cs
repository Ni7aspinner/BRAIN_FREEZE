using Microsoft.AspNetCore.Mvc;

namespace brainfreeze_new.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncController : ControllerBase
    {        
        private readonly ILogger<IncController> _logger;
        public IncController(ILogger<IncController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetData")]
        public ActionResult<Data> Get() 
        {
            Data sequence = new Data();
            for (int i = 0; i < sequence.level; i++)
            {
                sequence.createdList.Add(Random.Shared.Next(1, 10));
            }
            return Ok(sequence); 
        }
        [HttpPost(Name = "AddData")]
        public ActionResult<Data> Add([FromBody] Data sequence)
        {
            Console.WriteLine($"Data was sent back!!!{sequence.expectedList.Count}");
            if (sequence == null || sequence.createdList == null || sequence.expectedList == null || sequence.expectedList.Count != sequence.createdList.Count)
            {
                return BadRequest("Invalid data");
            }
            if (Check(sequence))
            {
                sequence.expectedList.Clear(); 
                sequence.level++;
                sequence.createdList.Add(Random.Shared.Next(1, 10)); 
                return Ok(sequence);
            }
            else
            {
                Data newSequence = new Data();
                for (int i = 0; i < newSequence.level; i++)
                {
                    newSequence.createdList.Add(Random.Shared.Next(1, 10));
                }
                return Ok(newSequence);
            }
        }
        private bool Check(Data sequence)
        { 
            for (int i = 0; i < sequence.level; i++)
            {
                if (sequence.createdList[i] != sequence.expectedList[i]) return false;
            }
            return true;
        }
    }
}
