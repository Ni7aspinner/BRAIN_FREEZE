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

        // Generates and returns Data type object with a list of random integers
        [HttpGet(Name = "GetData")]
        public ActionResult<Data> Get()
        {
            Data sequence = new Data();
            for (int i = 0; i < sequence.level; i++)
            {
                ModifyList(sequence);
            }
            return Ok(sequence);
        }

        // Validates and processes received data
        [HttpPost(Name = "AddData")]
        public ActionResult<Data> Add([FromBody] Data sequence)
        {
            Console.WriteLine($"Sent back data:\nArray size: {sequence.expectedList.Count}");
            if (sequence is null || sequence.createdList is null || sequence.expectedList is null || sequence.expectedList.Count != sequence.createdList.Count)
            {
                return BadRequest("Invalid data");
            }
            if (Check(sequence))
            {
                sequence.expectedList.Clear();
                sequence.level++;
                ModifyList(sequence);
                return Ok(sequence);
            }
            else
            {
                Data newSequence = new Data();
                for (int i = 0; i < newSequence.level; i++)
                {
                    ModifyList(newSequence);
                }
                Console.WriteLine($"Returning new sequence");
                return Ok(newSequence);
            }
        }

        // Checks to see if the sequence is ok. Has to deserialize from json if we want
        // to later use object to include also different types of information
        static private bool Check(Data sequence)
        {

            var createdListInts = sequence.createdList
                .Select(item => item is JsonElement jsonElement ? jsonElement.GetInt32() : (int)item)
                .ToList();

            var expectedListInts = sequence.expectedList
                .Select(item => item is JsonElement jsonElement ? jsonElement.GetInt32() : (int)item)
                .ToList();

            bool areEqual = createdListInts.SequenceEqual(expectedListInts);


            Console.Write("CreatedList: ");
            foreach (object item in sequence.createdList)
            {
                Console.Write(item + " ");
            }
            Console.Write("\nExpectedList: ");
            foreach (object item in sequence.expectedList)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine($"\nLists are equal: {areEqual}");
            return areEqual;
        }

        static private void ModifyList(Data sequence)
        {
            int createdNum = Random.Shared.Next(1, 10);
            object o = createdNum;
            sequence.createdList.Add(createdNum);
        }
    }
}