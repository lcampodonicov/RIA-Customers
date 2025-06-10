using RIACustomers.Database.Context;
using RIACustomers.Database.Managers;
using RIACustomers.Database.Models;

namespace RIACustomers.BusinessLayer.Toolkit;


public class CustomerHelper {

    public CustomerMan CustomerManager { get; set; }

    public CustomerHelper(CustomerMan Context){
        this.CustomerManager = Context;
    }

    public async Task<List<CustomerModel>> GetSortedCustomers(){

        var AllCustomers = await CustomerManager.GetAll();

        return Sorting.BubbleSort( AllCustomers, IsGreater );

    }
    
    private Boolean IsGreater( CustomerModel A, CustomerModel B ){

        if(Validations.IsGreater( A.FirstName, B.FirstName ))
            return true;

        if(Validations.IsGreater( B.FirstName, A.FirstName ))
            return false;

        return Validations.IsGreater( A.LastName, B.LastName );

    }

}
