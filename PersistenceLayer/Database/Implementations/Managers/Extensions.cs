using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RIACustomers.Database.Context;

namespace RIACustomers.Database.Managers;


public static class __ManagerExtensions__ {

    public static IServiceCollection RegisterPersistenceServices(this IServiceCollection This){

        This.AddDbContext<RIAContext>(
            Options =>
                Options.UseSqlite("Data Source=RIACustomers.db")
        );

        This.AddTransient<CustomerMan>();

        return This;

    }

}
