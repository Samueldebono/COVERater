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

        public DbSet<UserStats> UserStats { get; set; }
        public DbSet<UserEmails> UserEmails { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<UsersGuess> UsersGuess { get; set; }
        public DbSet<AuthUsers> AuthUsers { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<EmailLogs> EmailLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Image>()
                .HasMany(c=>c.SubImages)
                .WithOne(e=>e.Image)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
