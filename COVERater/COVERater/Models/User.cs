using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COVERater.API.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public Guid? HashUser { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? FinishedPhase1Utc { get; set; }
        public DateTime? FinishedPhase2Utc { get; set; }
        public DateTime? FinishedPhase3Utc { get; set; }
        public DateTime? TimePhase1 { get; set; }
        public DateTime? TimePhase2 { get; set; }
        public DateTime? TimePhase3 { get; set; }
        public int? PictureCycledPhase1 { get; set; }
        public int? PictureCycledPhase2 { get; set; }
        public int? PictureCycledPhase3 { get; set; }
        public decimal? FinishingPercentPhase1 { get; set; }
        public decimal? FinishingPercentPhase2 { get; set; }
        public decimal? FinishingPercentPhase3 { get; set; }

        public virtual ICollection<UsersGuess> Guesses { get; set; }
    }
}
