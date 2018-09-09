using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication001.Models
{
    public class CurrencyContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Rates> Rates { get; set; }
        public DbSet<User> Users { get; set; }

        public CurrencyContext(DbContextOptions<CurrencyContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
