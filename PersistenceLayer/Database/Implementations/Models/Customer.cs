
using System.ComponentModel.DataAnnotations;

namespace RIACustomers.Database.Models;


public class CustomerModel {

    [Key]
    public Int32? Id { get; set; }
    public String FirstName { get; set; } = "";
    public String LastName { get; set; } = "";
    public Int16 Age { get; set; }

}
