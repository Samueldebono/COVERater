using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COVERater.API.Bindings
{
    public class UserGuessBinding
    {
        public decimal GuessPercentage { get; set; }
        public int RoleId { get; set; }
        public int SubImageId { get; set; }
        public byte Phase { get; set; }
    }
}
