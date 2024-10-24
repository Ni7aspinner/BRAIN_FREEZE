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
            if (level == DifficultyLevel.Custom)
            {
                CustomList(data: sequence);
            } else
            {
                for (int i = 0; i < (int)level; i++)
                {
                    ModifyList(data: sequence);
                }
            }
            return Ok(new ResponseData(sequence, "Game started!"));
        }

        // Validates and processes received data
        [HttpPost(Name = "AddData")]
        public ActionResult<Data> Add([FromBody] Data sequence, DifficultyLevel level = DifficultyLevel.VeryEasy)
        {
            Console.WriteLine($"Sent back data:\nArray size: {sequence.expectedList.Count}");
            if (sequence is null || sequence.createdList is null || sequence.expectedList is null)
            {
                return BadRequest("Invalid data");
            }
            if (ShortCheck(sequence))
            {
                if(new Check(sequence).AreEqual)
                {
                    sequence.expectedList.Clear();
                    ModifyList(data: sequence);
                    return Ok(new ResponseData(sequence, "Congrats player!"));
                }
                return Ok(new ResponseData(sequence, "Proceed"));
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

        //Creates and applies custom list from Challenge.txt file
        static private void CustomList(Data data)
        {
            try
            {
                StreamReader reader = new("Challenge.txt");

                data.createdList.Clear();
                string? challengeData = reader.ReadLine();

                if (challengeData != null)
                {
                    List<object> challengeDataList = challengeData
                    .Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .Cast<object>().ToList();

                    data.createdList = challengeDataList;
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
            if (sequence.createdList.Count < sequence.expectedList.Count)
            {
                return false;
            }

            for (int i = 0; i < sequence.expectedList.Count; i++)
            {
                // Convert elements in both lists to integers using TryGetInt32
                if (sequence.createdList[i] is JsonElement createdElement
                    && sequence.expectedList[i] is JsonElement expectedElement)
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
                var createdListInts = sequence.createdList
                    .Select(item => item is JsonElement jsonElement ? jsonElement.GetInt32() : (int)item)
                    .ToList();

                var expectedListInts = sequence.expectedList
                    .Select(item => item is JsonElement jsonElement ? jsonElement.GetInt32() : (int)item)
                    .ToList();

                    AreEqual = createdListInts.SequenceEqual(expectedListInts);

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
                Console.WriteLine($"\nLists are equal: {AreEqual}");
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
        Impossible,
        Custom
    }
}