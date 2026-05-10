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
        public DbSet<FileType> FileTypes { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "Data Source=MemeCatalog.db");
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
