using System;
using System.ComponentModel.DataAnnotations;

namespace COVERater.API.Models
{
    public class EmailLogs
    {
        [Key]
        public int id { get; set; }

        public DateTime Time { get; set; }
        public string Status { get; set; }
        public string Response { get; set; }
        public string Email { get; set; }

        public string EmailSent { get; set; }
    }
}
