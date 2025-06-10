using System.ComponentModel.DataAnnotations;
using RIACustomers.BusinessLayer.Toolkit;

namespace RIACustomers.Rest.Models;


public class CustomersDTO : IValidatableObject {


    public List<CustomerDTO> Customers { get; set; } = new List<CustomerDTO>();
    
    /// <summary>
    /// Determines whether client will see both kinds of validations at once: Duplicates in input and duplicates in database
    /// </summary>
    public Boolean __ValidateBothAlways { get; set; } = false;

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext VC) {

        var DuplicatedInputErrors = GetIdsDuplicatedInputErrors();

        foreach(var DuplicatedInputError in DuplicatedInputErrors)
            yield return DuplicatedInputError;

        if( !__ValidateBothAlways && DuplicatedInputErrors.Count() > 0 )
            yield break;

        var AgesUnder18Errors = GetAgesUnder18Errors();

        if( AgesUnder18Errors.Count() > 0 ){
            foreach(var AgeUnder18Error in AgesUnder18Errors)
                yield return AgeUnder18Error;
            yield break;
        }

        var AlreadyInUseErrors = GetIdsAlreadyInUseErrors(VC);

        foreach(var AlreadyInUseError in AlreadyInUseErrors)
            yield return AlreadyInUseError;

    }

    public IEnumerable<ValidationResult> GetAgesUnder18Errors() =>
        Customers
            .Where( Customer => Customer.Age < 18 )
            .Select(
                Customer =>
                    new ValidationResult( $"Customer with id {Customer.Id} and age {Customer.Age} must be over 18 to be registered in this platform" )
            )
    ;

    

    public IEnumerable<ValidationResult> GetIdsAlreadyInUseErrors(ValidationContext VC){

        var ValidationService = (Validations) VC.GetService(typeof(Validations))!;

        var Ids = Customers
            .Where( Customer => Customer.Id != null )
            .Select( Customer => Customer.Id )
            .ToList()
        ;

        return ValidationService
            .GetMessagesForIDsInUse( Ids )
            .Select(
                IdInUseMessage =>
                    new ValidationResult( IdInUseMessage )
            )
        ;

    }

    public IEnumerable<ValidationResult> GetIdsDuplicatedInputErrors() => 
        Customers
            .GroupBy(
                Customer => Customer.Id
            )
            .Where(
                Group => Group.Count() >= 2
            )
            .Select(  
                Group => new ValidationResult($"Id {Group.Key} is duplicated in user input")
            )
    ;

}

public class CustomerDTO {

    [Required(ErrorMessage = "Please provide and Identifier")]
    public Int32? Id { get; set; }
    [Required(ErrorMessage = "Please provide a First Name")]
    public String? FirstName { get; set; }
    [Required(ErrorMessage = "Please provide a Last Name")]
    public String? LastName { get; set; }
    [Required(ErrorMessage = "Please provide an Age")]
    // [Range(18, Int16.MaxValue, ErrorMessage = "Customer with id {Id?} must be over 18 to be registered in this platform")]
    public Int16? Age { get; set; }

}
