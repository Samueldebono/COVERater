using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COVERater.API.Models
{
    public class AuthUsers
    {
        [Key]
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public byte RoleType { get; set; }

    }
}
