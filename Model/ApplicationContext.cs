using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employees.Model;
using Npgsql;

namespace employees.Model
{
    class ApplicationContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Card> Cards { get; set; }

        public ApplicationContext()
        {
            //Database.EnsureCreated();
            Database.CreateIfNotExists();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasMany(x => x.Cards)
                .WithRequired(x=>x.Employee);
        }
    }
}
