using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using COVERater.API.Models;

namespace COVERater.API.Dto
{
    [DataContract]
    public class UserDto
    {
        [DataMember(Name = "UserId")]
        public int UserId { get; set; }

        [DataMember(Name = "CreatedUtc")]
        public DateTime CreatedUtc { get; set; }
        [DataMember(Name = "FinishedPhase1Utc")]
        public DateTime? FinishedPhase1Utc { get; set; }
       [DataMember(Name = "FinishedPhase2Utc")]
        public DateTime? FinishedPhase2Utc { get; set; }
       [DataMember(Name = "FinishedPhase3Utc")]
        public DateTime? FinishedPhase3Utc { get; set; }
       [DataMember(Name = "TimePhase1")]
        public DateTime? TimePhase1 { get; set; }
       [DataMember(Name = "TimePhase2")]
        public DateTime? TimePhase2 { get; set; }
       [DataMember(Name = "TimePhase3")]
        public DateTime? TimePhase3 { get; set; }
       [DataMember(Name = "PictureCycledPhase1")]
        public int? PictureCycledPhase1 { get; set; }
       [DataMember(Name = "PictureCycledPhase2")]
        public int? PictureCycledPhase2 { get; set; }
       [DataMember(Name = "PictureCycledPhase3")]
        public int? PictureCycledPhase3 { get; set; }
       [DataMember(Name = "FinishingPercentPhase1")]
        public decimal? FinishingPercentPhase1 { get; set; }
       [DataMember(Name = "FinishingPercentPhase2")]
        public decimal? FinishingPercentPhase2 { get; set; }
       [DataMember(Name = "FinishingPercentPhase3")]
        public decimal? FinishingPercentPhase3 { get; set; }

        [DataMember(Name = "Guesses")]
        public virtual ICollection<UsersGuessDto> Guesses { get; set; }
    }
}
