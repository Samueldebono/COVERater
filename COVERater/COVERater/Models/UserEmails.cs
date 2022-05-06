using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COVERater.API.Models
{
    public class UserEmails
    {
        [Key]
        public int UserEmailId { get; set; }
        [Required]
        public DateTime CreatedUtc { get; set; }
        [Required] public string Email { get; set; }
        public bool PrizeSent { get; set; }

    }
}
