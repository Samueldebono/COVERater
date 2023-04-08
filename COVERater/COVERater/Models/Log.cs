using System;
using System.ComponentModel.DataAnnotations;

namespace COVERater.API.Models
{
    public class Log
    {
        [Key]
        public int LogId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }
        public string Function { get; set; }
        public string? Before { get; set; }
        public string? After { get; set; }
    }
}
