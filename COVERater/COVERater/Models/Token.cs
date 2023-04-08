using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COVERater.API.Models
{
    public class Token
    {
        [Key]
        public int TokenId { get; set; }

        public string UserGuid { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ExpiresTime { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
    }
}
