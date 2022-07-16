﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace COVERater.API.Models
{
    public class AuthUsers
    {
        [Key]
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public byte RoleType { get; set; }
        
        public byte ExperienceLevel { get; set; }
        public Guid? HashUser { get; set; }


        //[ForeignKey("User")]
        //protected virtual int? UserId { get; set; }

        //public void SetUserId(int? id)
        //{
        //    UserId = id;
        //}
        //public int GetUserId()
        //{
        //    return UserId ?? 0;
        //}
        public virtual List<UserStats> UserStats { get; set; }


    }
}
