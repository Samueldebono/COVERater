using COVERater.API.Models;
using Microsoft.EntityFrameworkCore;

namespace COVERater.API.Entities
{
    public class CoveraterContext : DbContext
    {
        public CoveraterContext()
        {
        }

        public CoveraterContext(DbContextOptions<CoveraterContext> options)
            : base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<UserEmails> UserEmails { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<UsersGuess> UsersGuess { get; set; }
        public DbSet<AuthUsers> AuthUsers { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
       
    }
}
