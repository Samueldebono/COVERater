using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COVERater.API.Bindings
{
    public class UserGuessBinding
    {

        public decimal GuessPercentage { get; set; }
        public int UserId { get; set; }
        public int ImageId { get; set; }
        public byte Phase { get; set; }
    }
}
