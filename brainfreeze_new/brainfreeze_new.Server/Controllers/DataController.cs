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

        // Generates and returns Data type object with a list of random integers
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

        // Validates and processes received data
        [HttpPost(Name = "AddData")]
        public ActionResult<Data> Add([FromBody] Data sequence)
        {
            Console.WriteLine($"Sent back data:\nArray size: {sequence.expectedList.Count}");
            if (sequence is null || sequence.createdList is null || sequence.expectedList is null)
            {
                return BadRequest("Invalid data");
            }
            if (Check(sequence))
            {
                if(sequence.expectedList.Count == sequence.createdList.Count) {
                    sequence.expectedList.Clear();
                    sequence.level++;
                    sequence.createdList.Add(Random.Shared.Next(1, 10));
                }
                return Ok(sequence);
            }
            else
            {
                Data newSequence = new Data();
                for (int i = 0; i < newSequence.level; i++)
                {
                    newSequence.createdList.Add(Random.Shared.Next(1, 10));
                }
                Console.WriteLine($"Returning new sequence");
                return Ok(newSequence);
            }
        }

        // Checks to see if the sequence is ok:)
        private bool Check(Data sequence)
        {
            for (int i = 0; i < sequence.expectedList.Count; i++)
            {
                if (sequence.createdList[i] != sequence.expectedList[i]) return false;
            }
            return true;
        }
    }
}
