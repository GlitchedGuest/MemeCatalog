using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemeCatalog.EFCore.Infrastructure
{
    public class MemeCatalogDBContext : DbContext
    {
        public DbSet<Meme> Memes { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "Data Source=MemeCatalog.db");
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
