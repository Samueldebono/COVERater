using System;

namespace COVERater.API.Models
{
    public static class Constants
    {
        [Flags]
        public enum Role
        {
            Student = 1,
            Experience = 2,
            Admin = 4
        }
    }
}
