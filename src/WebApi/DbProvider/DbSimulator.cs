using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.DbProvider
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("CustomersDb");
        }
    }
}
