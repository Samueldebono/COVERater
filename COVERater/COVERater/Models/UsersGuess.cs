using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int ImageId { get; set; }
        public byte Phase { get; set; }

        public virtual Image Image { set; get; }
        public virtual User User { set; get; }

    }
}
