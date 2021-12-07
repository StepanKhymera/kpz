using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class Context: DbContext
    {
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public Context(DbContextOptions<Context> options)
    : base(options)
        {
            Database.EnsureCreated();
        }

        public Context()
        {
        }
    }
}
