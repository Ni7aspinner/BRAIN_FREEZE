using System.Reflection.Metadata.Ecma335;

namespace brainfreeze_new.Server
{
    public class Data
    { 
        public List<object> createdList { get; set; } = new List<object>();
        public int level => createdList.Count;
        public List<object> expectedList { get; set; } = new List<object>();

    }

}