
using RIACustomers.Database.Managers;

namespace RIACustomers.BusinessLayer.Toolkit;


public class Validations {

    public CustomerMan CustomerManager { get; set; }

    public Validations(CustomerMan CustomerManager){

        this.CustomerManager = CustomerManager;

    }

    public static Boolean IsGreater(String? FirstString, String? SecondString){

        var MinLength = Math.Min( FirstString?.Length ?? 0, SecondString?.Length ?? 0 );

        for( var Index = 0; Index < MinLength ; Index++ ){

            var FirstCharacterFirstString = FirstString?[Index];
            var FirstCharacterSecondString = SecondString?[Index];

            if( FirstCharacterFirstString > FirstCharacterSecondString )
                return true;

            if( FirstCharacterFirstString < FirstCharacterSecondString )
                return false;

        }

        return FirstString?.Length > SecondString?.Length;

    }

    public List<String> GetMessagesForIDsInUse(List<Int32?> Ids) =>
        CustomerManager
            .GetIdsAlreadyInUse( Ids )
            .Select(
                IdInUse => $"Id {IdInUse} is already in use"
            )
            .ToList()
    ;

    

}
