using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace COVERater.API.Models
{
    public class UsersGuess
    {
        [Key]
        public int UsersGuessId { get; set; }
        public decimal GuessPercentage { get; set; }

        public int UserId { get; set; }
        public int SubImageId { get; set; }
        public byte Phase { get; set; }
        public DateTime GuessTimeUtc { get; set; }

        public virtual SubImage SubImage { set; get; }
        [ForeignKey("UserId")]
        public virtual UserStats UserStats { get; set; }



    }
}
