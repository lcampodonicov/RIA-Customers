using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RIACustomers.Database.Context;
using RIACustomers.Database.Models;

namespace RIACustomers.Database.Managers;


public class CustomerMan : EFCRUDAdapter<RIAContext> {

    public CustomerMan(RIAContext Context) : base(Context){
        this.Context = Context;
    }

    public async Task Create<DTOType>(DTOType NewCustomerDTO) =>
        await base.Create<DTOType, CustomerModel>( NewCustomerDTO )
    ;

    public async Task<List<CustomerModel>> Create<DTOType>(List<DTOType> NewCustomersDTO) =>
        await base.Create<DTOType, CustomerModel>( NewCustomersDTO )
    ;

    // public Task<Boolean> IsAnyIdAlreadyUsed(List<Int32> Ids) =>
    //     Context
    //         .Customers
    //         .AnyAsync(
    //             Customer =>
    //                 Ids.Contains( Customer.Id! )
    //         )
    // ;

    public List<Int32?> GetIdsAlreadyInUse(List<Int32?> Ids) =>
        Context
            .Customers
            .AsNoTracking()
            .Where(
                Customer =>
                    Ids.Contains( Customer.Id )
            )
            .Select(
                Customer =>
                    Customer.Id
            )
            .ToList()
    ;

    public async Task<List<CustomerModel>> GetAll() =>
        await 
            Context
                .Customers
                .ToListAsync()
    ;

    public async Task DeleteAll(){

        var All = await GetAll();
        Context.Customers.RemoveRange( All );
        Context.SaveChanges();

    }
    

}
