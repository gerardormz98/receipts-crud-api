using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudAPI.Models
{
    public class SimpleCrudDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Receipt> Receipts { get; set; }

        public SimpleCrudDBContext(DbContextOptions<SimpleCrudDBContext> options) 
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User { ID = 1, Email = "admin@test.com", IsAdmin = true });
            modelBuilder.Entity<Supplier>().HasData(new Supplier { ID = 1, Name = "Fake Supplier #1", Phone = "123456789" });
            modelBuilder.Entity<Receipt>().HasData(new Receipt { ID = 1, UserID= 1, SupplierID = 1, Date = DateTime.Now, Amount = 1000, Comments = "First Receipt!" });
        }
    }
}
