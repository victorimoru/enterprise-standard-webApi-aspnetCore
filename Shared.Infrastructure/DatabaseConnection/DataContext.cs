
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Infrastructure.DatabaseConnection
{
   public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }

    }
}
