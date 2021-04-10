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
    }
}
