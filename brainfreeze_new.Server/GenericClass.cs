using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace brainfreeze_new.Server
{
    public class GenericClass<X> where X : struct, IEquatable<X>
    {
        private List<X> _createdList = [];

        public List<X> CreatedList
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
        public int Level => CreatedList.Count;
        public List<X> ExpectedList { get; set; } = [];

        public int Difficulty { get; set; }

        public bool Equals()
        {
            if (_createdList.Count != ExpectedList.Count) return false;

            for (int i = 0; i < _createdList.Count; i++) { if (!_createdList[i].Equals(ExpectedList[i])) return false; }

            return true;
        }
    }

}