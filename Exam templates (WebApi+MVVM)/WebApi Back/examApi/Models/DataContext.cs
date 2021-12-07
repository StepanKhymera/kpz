using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace examApi.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Song> Songs { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Song>().HasData(
                new Song { SongID=1, Artist = "Vinnik", Name ="Vovchica", FilePath="C:\\songs\\vovk.mp3"},
                new Song { SongID = 2,Artist = "Stepan", Name = "Sucker", FilePath = "C:\\songs\\dungeonsuck.mp3" }
            );
        }
    }
}
