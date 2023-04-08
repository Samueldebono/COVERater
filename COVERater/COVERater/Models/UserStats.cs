using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COVERater.API.Models
{
    public class UserStats
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public DateTime CreatedUtc { get; set; }
        public DateTime? FinishedPhaseUtc { get; set; }
        public DateTime? TimePhase { get; set; }
        public int? PictureCycledPhase { get; set; }
        public decimal? FinishingPercentPhase { get; set; }
        public byte Phase { get; set; }

        public bool? Deleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public virtual ICollection<UsersGuess> Guesses { get; set; }
    }
}
