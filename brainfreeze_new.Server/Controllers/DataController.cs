using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace brainfreeze_new.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> _logger;

        public DataController(ILogger<DataController> logger)             
        {
            _logger = logger;
        }
        public record ResponseData(Data Data, string Message);

        // Generates and returns Data type object with a list of random integers
        [HttpGet(Name = "GetData")]
        public async Task<ActionResult<Data>> Get([FromQuery] DifficultyLevel level)
        {
            Data sequence = new();
            if (level == DifficultyLevel.Custom)
            {
                await CustomList(sequence);
            } else
            {
                for (int i = 0; i < (int)level; i++)
                {
                    ModifyList(sequence, max: 10);
                }
            }
            return Ok(new ResponseData(sequence, "Game Started!"));
        }

        // Validates and processes received data
        [HttpPost(Name = "AddData")]
        public async Task<ActionResult<Data>> Add([FromBody] Data? sequence)
        {
            if (sequence is null || sequence.CreatedList is null || sequence.ExpectedList is null)
            {
                return BadRequest("Invalid data");
            }
            int level = sequence.Difficulty;
            Console.WriteLine($"Sent back data:\nArray size: {sequence.ExpectedList.Count}");
            if (ShortCheck(sequence))
            {
                if (new Check(sequence).AreEqual)
                {
                    sequence.ExpectedList.Clear();
                    ModifyList(sequence);
                    return Ok(new ResponseData(sequence, "Congrats player!"));
                }
                return Ok(new ResponseData(sequence, "Proceed"));
            }
            else
            {
                Data newSequence = new();
                if(level == (int)DifficultyLevel.Custom)
                {
                    await CustomList(newSequence);
                }
                else
                {
                    for (int i = 0; i < (int)level; i++)
                    {
                        ModifyList(newSequence);
                    }
                }
                Console.WriteLine($"Returning new sequence");
                return Ok(new ResponseData(newSequence, "Loser!"));
            }
        }

        static private void ModifyList(Data data, int min=1, int max=10)
        {
            int createdNum = Random.Shared.Next(min, max);
            object o = createdNum;
            data.CreatedList.Add(o);
        }

        //Creates and applies custom list from Challenge.txt file
        static private async Task CustomList(Data data)
        {
            try
            {
                using StreamReader reader = new("Challenge.txt");
                data.CreatedList.Clear();
                string? challengeData = await reader.ReadLineAsync();
                char[] separator = [' ', ','];

                if (challengeData != null)
                {
                    List<object> challengeDataList = challengeData
                    .Split(separator, StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .Cast<object>().ToList();

                    data.CreatedList = challengeDataList;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        static public bool ShortCheck(Data sequence)
        {
            // Ensure both lists are non-null
            if (sequence.CreatedList == null || sequence.ExpectedList == null)
            {
                return false;
            }

            // Ensure CreatedList is at least as long as ExpectedList
            if (sequence.CreatedList.Count < sequence.ExpectedList.Count)
            {
                return false;
            }

            // Convert ExpectedList to integers
            var expectedListInts = sequence.ExpectedList
                .Select(item => item is JsonElement jsonElement ? jsonElement.GetInt32() : (int)item)
                .ToList();

            // Convert CreatedList to integers
            var createdListInts = sequence.CreatedList
                .Select(item => item is JsonElement jsonElement ? jsonElement.GetInt32() : (int)item)
                .ToList();

            // Compare corresponding elements
            for (int i = 0; i < sequence.ExpectedList.Count; i++)
            {
                if (expectedListInts[i] != createdListInts[i])
                {
                    return false;
                }
            }

            return true; // Return true if all comparisons passed
        }

        // Checks to see if the sequence is ok. Has to deserialize from json if we want
        // to later use object to include also different types of information
        public readonly struct Check
        {
           public bool AreEqual { get; } = false;
            public Check(Data sequence)
            {

                IEnumerable<int> expectedListInts = sequence.ExpectedList
                    .Select(item => item is JsonElement jsonElement ? jsonElement.GetInt32() : (int)item)
                    .ToList();

                IEnumerable<int> createdListInts = sequence.CreatedList
                    .Select(item => item is JsonElement jsonElement ? jsonElement.GetInt32() : (int)item)
                    .ToList();

                AreEqual = createdListInts.SequenceEqual(expectedListInts);

                //very random implementation of extension method. 
                //in this case Words is extended with WordsExtended
                DateTime today = DateTime.Now;

                Console.Write(today.Created());
                foreach (object item in sequence.CreatedList)
                {
                    Console.Write(item + today.Space());
                }
                Console.Write(today.Expected());
                foreach (object item in sequence.ExpectedList)
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine($"\nLists are equal: {AreEqual}");
            }
        }
    }

    public enum DifficultyLevel
    {
        MainStart = 1,
        VeryEasy = 4,
        Easy = 5,
        Normal = 6,
        Hard = 7,
        Nightmare = 8,
        Impossible = 9,
        Custom = 0
    }
}