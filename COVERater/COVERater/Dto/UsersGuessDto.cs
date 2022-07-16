using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using COVERater.API.Models;

namespace COVERater.API.Dto
{
    [DataContract]
    public class UsersGuessDto
    {
        [DataMember(Name = "UsersGuessId")]
        public int UsersGuessId { get; set; }

        [DataMember(Name = "GuessPercentage")]
        public decimal GuessPercentage { get; set; }

        [DataMember(Name = "UserId")]

        public int UserId { get; set; }

        [DataMember(Name = "ImageId")]
        public int ImageId { get; set; }
        [DataMember(Name = "Phase")]
        public byte Phase { get; set; }

        [DataMember(Name = "Image")]
        public virtual Image Image { set; get; }

        [DataMember(Name = "User")]
        public virtual UserStats UserStats { set; get; }

    }
}
