using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COVERater.API.Bindings
{
    public class CreateUserEmailBinding
    {
        public DateTime CreatedUtc { get; set; }
         public string Email { get; set; }
        public bool? PrizeSent { get; set; }
    }
}
