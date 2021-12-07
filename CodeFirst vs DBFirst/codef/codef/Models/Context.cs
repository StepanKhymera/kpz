using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codef
{
     class Context : DbContext
    {
        public Context()
        {
            this.Database.Connection.ConnectionString = "Data Source=DESKTOP-4IJ8AA6;Integrated Security=True";
        }
        public virtual DbSet<Policy> Policies { get; set; }
    }
}
