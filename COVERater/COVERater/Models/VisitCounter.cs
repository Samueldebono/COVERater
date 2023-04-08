using System.ComponentModel.DataAnnotations;

namespace COVERater.API.Models
{
    public class VisitCounter
    {
        [Key]
        public int Id { get; set; }
        public int Count { get; set; }
    }
}
