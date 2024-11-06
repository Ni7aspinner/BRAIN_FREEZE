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
        public record ResponseData(Data Data, string Message);
        [HttpGet(Name = "GetDataNRG")]
        public ActionResult<Data> Get(DifficultyLevel level = DifficultyLevel.VeryEasy)
        {
            Data sequence = new Data();
            if (level == DifficultyLevel.Custom)
            {
                CustomList(sequence);
            }
            else
            {
                for (int i = 0; i < (int)level; i++)
                {
                    ModifyList(sequence, max: 25);
                }
            }
            return Ok(new ResponseData(sequence, "Game started!"));
        }

        // Validates and processes received data
        [HttpPost(Name = "AddDataNRG")]
        public ActionResult<Data> Add([FromBody] Data sequence, DifficultyLevel level = DifficultyLevel.VeryEasy)
        {
            Console.WriteLine($"Sent back data:\nArray size: {sequence.ExpectedList.Count}");
            if (sequence is null || sequence.CreatedList is null || sequence.ExpectedList is null)
            {
                return BadRequest("Invalid data");
            }
            if (ShortCheck(sequence))
            {
                if (new Check(sequence).AreEqual)
                {
                    CreateLists(sequence, level:sequence.Level+1);
                    return Ok(new ResponseData(sequence, "Congrats player!"));
                }
                return Ok(new ResponseData(sequence, "Proceed"));
            }
            else
            {
                Data newSequence = new();
                CreateLists(newSequence);
                Console.WriteLine($"Returning new sequence");
                return Ok(new ResponseData(newSequence, "Loser!"));
            }
        }
        static private void ModifyList(Data data, int min = 0, int max = 25)
        {
            int createdNum = Random.Shared.Next(min, max);
            while (data.CreatedList.Contains(createdNum)) createdNum = Random.Shared.Next(min, max); //temporary
            object o = createdNum;
            data.CreatedList.Add(o);
        }
        static private void CreateLists(Data newSequence, int min = 0, int max = 25, int level=4)
        {
            newSequence.CreatedList.Clear();
            newSequence.ExpectedList.Clear();
            for (int i = 0; i < (int)level; i++)
            {
                ModifyList(newSequence, max: 25);
            }
        }

        //Creates and applies custom list from Challenge.txt file
        static private void CustomList(Data data)
        {
            try
            {
                StreamReader reader = new("Challenge.txt");

                data.CreatedList.Clear();
                string? challengeData = reader.ReadLine();

                if (challengeData != null)
                {
                    List<object> challengeDataList = challengeData
                    .Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
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

}