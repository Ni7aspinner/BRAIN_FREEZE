using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace brainfreeze_new.Server.Models
{
    public class Scoreboard
    {
        [Key]
        public int id { get; set; }

        public int place { get; set; }

        [Column(TypeName ="nvarchar(100)")]
        public String username { get; set; }

        public int simonScore { get; set; }

        public int cardflipScore { get; set; }

        public int nrgScore { get; set; }

    }
}
