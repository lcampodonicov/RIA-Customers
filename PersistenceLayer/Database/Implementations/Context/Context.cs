using Microsoft.EntityFrameworkCore;
using RIACustomers.Database.Models;

namespace RIACustomers.Database.Context;

public class RIAContext : DbContext {

    public RIAContext(DbContextOptions<RIAContext> Options) : base(Options){}

    public DbSet<CustomerModel> Customers { get; set; }

}
