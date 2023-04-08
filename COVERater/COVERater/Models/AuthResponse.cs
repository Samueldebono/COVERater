using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COVERater.API.Models
{
    public class AuthResponse
    {
        public string BearerToken { get; set; }
        public byte RoleType { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int AccessId { get; set; }
        public int RoleId { get; set; }
        public List<UserStats> UserStats { get; set; }

        public string UserName { get; set; }
    }

}
