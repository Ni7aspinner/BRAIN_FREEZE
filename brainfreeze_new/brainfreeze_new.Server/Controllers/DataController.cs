using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        public record ResponseData(Data Data, string Message);
        // Generates and returns Data type object with a list of random integers
        [HttpGet(Name = "GetData")]
        public ActionResult<Data> Get(DifficultyLevel level = DifficultyLevel.VeryEasy)
        {
            Data sequence = new Data();
            for (int i = 0; i < (int)level; i++)
            {
                ModifyList(data: sequence);
            }
            return Ok(new ResponseData(sequence, "Game started!"));
        }

        // Validates and processes received data
        [HttpPost(Name = "AddData")]
        public ActionResult<Data> Add([FromBody] Data sequence, DifficultyLevel level = DifficultyLevel.VeryEasy)
        {
            Console.WriteLine($"Sent back data:\nArray size: {sequence.expectedList.Count}");
            if (sequence is null || sequence.createdList is null || sequence.expectedList is null || sequence.expectedList.Count != sequence.createdList.Count)
            {
                return BadRequest("Invalid data");
            }
            if (new Check(sequence).areEqual)
            {
                sequence.expectedList.Clear();
                ModifyList(data: sequence);
                return Ok(new ResponseData(sequence,"Congrats player!"));
            }
            else
            {
                Data newSequence = new Data();
                for (int i = 0; i < (int)level; i++)
                {
                    ModifyList(data: newSequence);
                }
                Console.WriteLine($"Returning new sequence");
                return Ok(new ResponseData(newSequence, "Loser!"));
            }
        }

        static private void ModifyList(Data data)
        {
            int createdNum = Random.Shared.Next(1, 10);
            object o = createdNum;
            data.createdList.Add(createdNum);
        }

        // Checks to see if the sequence is ok. Has to deserialize from json if we want
        // to later use object to include also different types of information
        public struct Check
        {
           public bool areEqual { get; } = false;
            public Check(Data sequence)
            {
                var createdListInts = sequence.createdList
                    .Select(item => item is JsonElement jsonElement ? jsonElement.GetInt32() : (int)item)
                    .ToList();

                var expectedListInts = sequence.expectedList
                    .Select(item => item is JsonElement jsonElement ? jsonElement.GetInt32() : (int)item)
                    .ToList();

                areEqual = createdListInts.SequenceEqual(expectedListInts);

                //very random implementation of extension method. 
                //in this case Words is extended with WordsExtended
                Words words = new Words();

                Console.Write(words.Created());
                foreach (object item in sequence.createdList)
                {
                    Console.Write(item + words.Space());
                }
                Console.Write(words.Expected());
                foreach (object item in sequence.expectedList)
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine($"\nLists are equal: {areEqual}");
            }
        }
    }
    public enum DifficultyLevel
    {
        VeryEasy = 4,
        Easy,
        Normal,
        Hard,
        Nightmare,
        Impossible
    }
}