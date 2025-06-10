using RIACustomers.BusinessLayer.Toolkit;
using RIACustomers.Rest.Models;

namespace RIACustomers.Rest.Helpers;


public class ValidationHelpers {

    public static Boolean IsGreater( CustomerDTO A, CustomerDTO B ){

        if(Validations.IsGreater( A.LastName, B.LastName ))
            return true;

        if(Validations.IsGreater( B.LastName, A.LastName ))
            return false;

        return Validations.IsGreater( A.FirstName, B.FirstName );

    }

}
