using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace MemeCatalogDatabase
{
    public class MemeContext : DbContext
    {
        public DbSet<Meme> Memes { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>().Property(t => t.name).IsRequired();
            modelBuilder.Entity<Meme>().Property(t => t.path).IsRequired();
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "Data Source=MemeCatalog.db");
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
