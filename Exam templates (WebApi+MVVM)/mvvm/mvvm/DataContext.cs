using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace mvvm
{
    public partial class DataContext : DbContext
    {
        public DataContext()
            : base("name=DataContext")
        {

        }

        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(e => e.price)
                .HasPrecision(8, 2);
        }
    }
}
