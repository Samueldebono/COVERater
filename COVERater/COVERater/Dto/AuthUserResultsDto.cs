using System.Collections.Generic;
using System.Runtime.Serialization;
using COVERater.API.Models;

namespace COVERater.API.Dto
{
    [DataContract]
    public class AuthUserResultsDto
    {
        [DataMember(Name = "RoleId")]
        public int RoleId { get; set; }
        [DataMember(Name = "UserName")]
        public string UserName { get; set; }
        [DataMember(Name = "Email")]
        public string Email { get; set; }
        [DataMember(Name = "RoleType")]
        public byte RoleType { get; set; }
        [DataMember(Name = "ExperienceLevel")]
        public byte ExperienceLevel { get; set; }
        [DataMember(Name = "UserStats")]
        public List<UserStats> UserStats { get; set; }


    }
}
