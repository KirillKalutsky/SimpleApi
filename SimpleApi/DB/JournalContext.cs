using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleApi.Core;

namespace SimpleApi.DB
{
    public class JournalContext:DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Article> Articles { get; set; }

        public JournalContext(DbContextOptions<JournalContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasMany(author => author.Works)
                .WithMany(article => article.Creators);
        }
    }
}
