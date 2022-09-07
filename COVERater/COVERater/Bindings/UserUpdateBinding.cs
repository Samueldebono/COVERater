using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace COVERater.API.Bindings
{
    [DataContract]
    public class UpdateUserBinding
    {
        [DataMember(Name = "FinishedUtc")]
        public DateTime? FinishedUtc { get; set; }
        [DataMember(Name = "FinishingPercent")]
        public decimal FinishingPercent { get; set; }
        [DataMember(Name = "PictureCycled")]
        public int PictureCycled { get; set; }
        [DataMember(Name = "Time")]
        public DateTime Time { get; set; }
        [DataMember(Name = "Phase")]
        public byte Phase { get; set; }
    }
    [DataContract]
    public class CreateUserBinding
    {
        [DataMember(Name = "RoleId")]
        public int RoleId { get; set; }    
        
        [DataMember(Name = "Phase")]
        public byte Phase { get; set; }

    }
}
