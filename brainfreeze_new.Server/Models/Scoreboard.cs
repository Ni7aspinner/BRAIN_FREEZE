using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace brainfreeze_new.Server.Models
{
    public class Scoreboard
    {
        [Key]
        public int Id { get; set; }

        public int Place { get; set; }

        [Column(TypeName ="nvarchar(100)")]
        public String? Username { get; set; }

        public int SimonScore { get; set; }

        public int CardflipScore { get; set; }

        public int NrgScore { get; set; }

    }
}
