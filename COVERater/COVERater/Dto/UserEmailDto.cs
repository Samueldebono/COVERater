using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COVERater.API.Dto
{
    public class UserEmailDto
    {
        public int UserEmailId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string Email { get; set; }
        public bool PrizeSent { get; set; }
    }
}
