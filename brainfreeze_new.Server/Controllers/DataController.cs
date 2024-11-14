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
        public ActionResult<Data> Get([FromQuery] DifficultyLevel level)
        {
            Data sequence = new Data();
            if (level == DifficultyLevel.Custom)
            {
                CustomList(sequence);
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
        public ActionResult<Data> Add([FromBody] Data sequence)
        {
            int level = sequence.Difficulty;
            Console.WriteLine($"Sent back data:\nArray size: {sequence.ExpectedList.Count}");
            if (sequence is null || sequence.CreatedList is null || sequence.ExpectedList is null)
            {
                return BadRequest("Invalid data");
            }
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
                    CustomList(newSequence);
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
        static private void CustomList(Data data)
        {
            try
            {
                using StreamReader reader = new("Challenge.txt");
                data.CreatedList.Clear();
                string? challengeData = reader.ReadLine();
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
        
        private static bool ShortCheck(Data sequence)
        {
            // Ensure both lists are of equal length or that createdList is longer than expectedList
            if (sequence.CreatedList.Count < sequence.ExpectedList.Count)
            {
                return false;
            }

            // Handles starting list of 1
            if (sequence.CreatedList.Count == 1 && sequence.ExpectedList.Count == 1 && new Check(sequence).AreEqual)
            {
                return true;
            }

            for (int i = 0; i < sequence.ExpectedList.Count; i++)
            {
                // Convert elements in both lists to integers using TryGetInt32
                if (sequence.CreatedList[i] is JsonElement createdElement
                    && sequence.ExpectedList[i] is JsonElement expectedElement)
                {
                    if (createdElement.ValueKind == JsonValueKind.Number
                        && expectedElement.ValueKind == JsonValueKind.Number)
                    {
                        int createdInt = createdElement.GetInt32();
                        int expectedInt = expectedElement.GetInt32();

                        // Compare the two integers
                        if (createdInt != expectedInt) return false;
                    }
                    else
                    {
                        // If either is not a number, return false
                        return false;
                    }
                }
                else
                {
                    // If either element is not a JsonElement, return false
                    return false;
                }
            }

            // Return true if all comparisons passed
            return true;
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

                IEnumerable<int> createdListInts =
                   from created in sequence.CreatedList
                   where created is JsonElement
                   select ((JsonElement)created).GetInt32();

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