using System;
namespace brainfreeze_new.Server.Controllers
{
    public static class WordsExtended
    {
        
        public static String Expected(this DateTime W)
        {
            return "\nExpectedList: ";
        }
         public static String Created(this DateTime W)
        {
            return "CreatedList: ";
        }
        public static String Space(this DateTime W)
        {
            return " ";
        }
    }
}
