using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace brainfreeze_new.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ButtonController : ControllerBase
    {
        private readonly IncController incController;
        private List<int> expectedList = new List<int>();
        private List<int> createdList = new List<int>();
        

        [HttpPost]
        public IActionResult ReceiveButtonPress([FromBody] ButtonData data)
        {
            expectedList = data.number;
            Console.WriteLine($"Button {data.ButtonId} pressed at {data.Timestamp}");
            Console.WriteLine($"{expectedList[0]} {expectedList[1]} {expectedList[2]} {expectedList[3]}");
            if (createdList.Count() < expectedList.Count())
            {
               
                createdList.Add(data.ButtonId);
                Console.WriteLine($"Button {createdList[createdList.Count-1]} ");
                for(int i=0; i<createdList.Count; i++)
                {
                    Console.Write ($"{createdList[i]} ");
                }
                
            }
            else
            {
                Console.WriteLine($"Full");
                if (createdList.SequenceEqual(expectedList))
                {
                    //var newData = incController.Add();
                    createdList.Clear();
                    Console.WriteLine($"Lists matched!");
                    //return Ok(newData);
                }
                else
                {
                    createdList.Clear();
                }
            }
            
            return Ok(data);
        }
    }
}
