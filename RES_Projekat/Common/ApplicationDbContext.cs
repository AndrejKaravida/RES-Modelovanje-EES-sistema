using Common.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base()
        {
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public DbSet<Generator> Generators { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<LocalController> LocalControllers { get; set; }
        public DbSet<SystemController> SystemControllers { get; set; }
     
    }
}
