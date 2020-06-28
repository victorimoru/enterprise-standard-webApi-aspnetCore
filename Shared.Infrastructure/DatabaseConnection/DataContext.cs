using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Entities;

namespace Shared.Infrastructure.DatabaseConnection
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Token> WebTokens { get; set; }
        public DbSet<Like> Likes { get; set; }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Like>(new LikeEntityConfiguration());

        }

    }
}
