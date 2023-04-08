using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COVERater.API.Models
{
    public class Image
    {
        [Key]
        public int ImageId { get; set; }
        public Guid CloudinaryId { get; set; }
        public DateTime AddedUtc { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        
        public bool Delete { get; set; }
        public DateTime? DeletedUtc { get; set; }
        public ImageType Type { get; set; }
        public virtual List<SubImage> SubImages { get; set; }

    }

    public enum ImageType
    {
        Terrestrial,
        Marine
    }
}
