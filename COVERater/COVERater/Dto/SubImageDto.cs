using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COVERater.API.Models;

namespace COVERater.API.Dto
{
    public class SubImageDto
    {
        public int SubImageId { get; set; }
        public Guid CloudinaryId { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public bool Delete { get; set; }
        public decimal CoverageRate { get; set; }
        
    }
}
