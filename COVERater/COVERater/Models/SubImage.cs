using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COVERater.API.Models
{
    public class SubImage
    {
        [Key]
        public int SubImageId { get; set; }
        public Guid CloudinaryId { get; set; }
        public DateTime AddedUtc { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public bool Delete { get; set; }
        public decimal CoverageRate { get; set; }
        public DateTime? DeletedUtc { get; set; }

        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}
