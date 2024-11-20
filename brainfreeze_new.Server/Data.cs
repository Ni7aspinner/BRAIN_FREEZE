using System.Reflection.Metadata.Ecma335;

namespace brainfreeze_new.Server
{
    public class Data
    {

        private List<object> _createdList = [];     // Let's use _ for private fields:)
        
        public List<object> CreatedList
        {
            get
            {
                return _createdList;
            }
            set
            {
                _createdList = value;
            }
        }

        // Also possible to rewrite it like this:
        // public List<object> createdList { get; set; } = new List<object>();     
        public int Level => CreatedList.Count;
        public List<object> ExpectedList { get; set; } = [];

        public int Difficulty {  get; set; }

    }

}