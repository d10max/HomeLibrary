using homeLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homeLibrary.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Mylibrary.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, FullName = "Taras Shevchenko" },
                new Author { Id = 2, FullName = "Steven King" },
                new Author { Id = 3, FullName = "George Orwell" }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Name = "Kobzar", Year = 1840, AuthorId = 1 },
                new Book { Id = 2, Name = "Gaydamaky", Year = 1841, AuthorId = 1 },
                new Book { Id = 3, Name = "It", Year = 1986, AuthorId = 2 },
                new Book { Id = 4, Name = "Shining", Year = 1977, AuthorId = 2 },
                new Book { Id = 5, Name = "1984", Year = 1949, AuthorId = 3 },
                new Book { Id = 6, Name = "Animal farm", Year = 1952, AuthorId = 3 }
            );
        }
    }
}
