using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using COVERater.API.Models;

namespace COVERater.API.Dto
{
    [DataContract]
    public class UsersResultDto
    {
        [DataMember(Name = "UserId")]
        public int UserId { get; set; }

        [DataMember(Name = "CreatedUtc")]
        public DateTime CreatedUtc { get; set; }
        [DataMember(Name = "FinishedPhaseUtc")]
        public DateTime? FinishedPhaseUtc { get; set; }

       [DataMember(Name = "TimePhase")]
        public DateTime? TimePhase { get; set; }

       [DataMember(Name = "PictureCycledPhase")]
        public int? PictureCycledPhase { get; set; }
        
       [DataMember(Name = "FinishingPercentPhase")]
        public decimal? FinishingPercentPhase { get; set; }        
       [DataMember(Name = "Phase")]
       public byte Phase { get; set; }

       [DataMember(Name = "Email")]
       public string Email { get; set; }

       [DataMember(Name = "Role")] public int Role { get; set; }

       [DataMember(Name = "Experience")]
       public int Experience { get; set; }

        [DataMember(Name = "Guesses")]
        public virtual ICollection<UsersGuess> Guesses { get; set; }
    }
}
