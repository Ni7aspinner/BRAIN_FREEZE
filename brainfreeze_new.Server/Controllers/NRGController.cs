using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace brainfreeze_new.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NRGController : ControllerBase
    {
        private readonly ILogger<NRGController> _logger;

        public NRGController(ILogger<NRGController> logger)
        {
            _logger = logger;
        }
        public record ResponseData(GenericClass<int> data, string Message);
        [HttpGet(Name = "GetDataNRG")]
        public ActionResult<GenericClass<int>> Get(DifficultyLevel level = DifficultyLevel.VeryEasy)
        {
            GenericClass<int> sequence = new GenericClass<int>();
            for (int i = 0; i < (int)level; i++)
            {
               ModifyList(sequence, max: 25);
            }
      
            return Ok(new ResponseData(sequence, "Game started!"));
        }
        [HttpPost(Name = "AddDataNRG")]
        public ActionResult<GenericClass<int>> Add([FromBody] GenericClass<int> sequence, DifficultyLevel level = DifficultyLevel.VeryEasy)
        {
            Console.WriteLine($"Sent back data:\nArray size: {sequence.ExpectedList.Count}");
            if (sequence is null || sequence.CreatedList is null || sequence.ExpectedList is null)
            {
                return BadRequest("Invalid data");
            }
            if (sequence.Equals())
            {
                CreateLists(sequence, level:sequence.Level+1);
                return Ok(new ResponseData(sequence, "Congrats player!"));
            }
            else
            {
                GenericClass<int> newSequence = new();
                CreateLists(newSequence);
                Console.WriteLine($"Returning new sequence");
                return Ok(new ResponseData(newSequence, "Loser!"));
            }
        }
        static private void ModifyList(GenericClass<int> data, int min = 0, int max = 25)
        {
            int createdNum = Random.Shared.Next(min, max);
            while (data.CreatedList.Contains(createdNum)) createdNum = Random.Shared.Next(min, max);
            data.CreatedList.Add(createdNum);
        }
        static private void CreateLists(GenericClass<int> newSequence, int min = 0, int max = 25, int level=4)
        {
            newSequence.CreatedList.Clear();
            newSequence.ExpectedList.Clear();
            for (int i = 0; i < (int)level; i++)
            {
                ModifyList(newSequence, max: 25);
            }
        }
        static private void checkValue(GenericClass<int> data, int value)
        {
            if (data.FirstElementEqualTo(value)) Console.WriteLine("yay");
        }
        
    }

}