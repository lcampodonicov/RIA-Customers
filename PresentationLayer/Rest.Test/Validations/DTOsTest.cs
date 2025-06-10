
using System.ComponentModel.DataAnnotations;
using BusinessLayer.Toolkit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using RIACustomers.BusinessLayer.Toolkit;
using RIACustomers.Database.Context;
using RIACustomers.Database.Managers;
using RIACustomers.Database.Models;
using RIACustomers.Rest.Helpers;
using RIACustomers.Rest.Models;

namespace RIACustomers.Rest.Test.ValidationsTest;

public class DTOsTest {

    public RIAContext Context { get; set; } = null!;
    public ServiceProvider ServiceProvider { get; set; } = null!;

    [SetUp]
    public void GeneralSetup(){

        var Services = new ServiceCollection();

        Services.RegisterPersistenceServices();

        ServiceProvider = Services.BuildServiceProvider();

        Context = ServiceProvider.GetRequiredService<RIAContext>();

        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();

    }

    private void IdsNotAlreadyInUse_Setup(){

        Context.Add(new CustomerModel(){
            Id = 1
        });

        Context.SaveChanges();

    }

    [Test]
    public void IdsNotAlreadyInUse() {

        IdsNotAlreadyInUse_Setup();

        var DTO = new CustomersDTO(){
            Customers = new List<CustomerDTO>(){
                new CustomerDTO(){
                    Id = 1
                }
            }
        };

        var Validations = new List<ValidationResult>();

        var IsValid = Validator.TryValidateObject(
            DTO,
            new ValidationContext(DTO, serviceProvider: ServiceProvider, items: null),
            Validations,
            validateAllProperties: true
        );

        Assert.That(IsValid, Is.False);
        Assert.That(Validations, Has.Count.EqualTo(1));
        Assert.That(Validations[0].ErrorMessage, Is.EqualTo("Id 1 is already in use"));

    }

    [Test]
    public void IdsDuplicatedInUserInput(){

        var DTO = new CustomersDTO(){
            Customers = new List<CustomerDTO>(){
                new CustomerDTO(){
                    Id = 1
                },
                new CustomerDTO(){
                    Id = 1
                }
            },
        };

        var Validations = new List<ValidationResult>();

        var IsValid = Validator.TryValidateObject(
            DTO,
            new ValidationContext(DTO, items: null),
            Validations,
            validateAllProperties: true
        );

        Assert.That(IsValid, Is.False);
        Assert.That(Validations, Has.Count.EqualTo(1));
        Assert.That(Validations[0].ErrorMessage, Is.EqualTo("Id 1 is duplicated in user input"));

    }

    [Test]
    public void IsRequired(){

        var DTO = new CustomerDTO();

        var Validations = new List<ValidationResult>();

        var IsValid = Validator.TryValidateObject(
            DTO,
            new ValidationContext(DTO, serviceProvider: ServiceProvider, items: null),
            Validations,
            validateAllProperties: true
        );

        Assert.That(IsValid, Is.False);
        Assert.That(Validations, Has.Count.EqualTo(4));

    }

    [Test]
    public void AgeIsAbove18(){

        var DTO = new CustomerDTO(){
            Id = 99,
            FirstName = "Test",
            LastName = "Test",
            Age = 17
        };

        var Validations = new List<ValidationResult>();

        var IsValid = Validator.TryValidateObject(
            DTO,
            new ValidationContext(DTO, serviceProvider: ServiceProvider, items: null),
            Validations,
            validateAllProperties: true
        );

        Assert.That(IsValid, Is.False);
        Assert.That(Validations, Has.Count.EqualTo(1));
        Assert.That(Validations[0].ErrorMessage, Is.EqualTo("Customer must be over 18 to be registered in this platform"));

    }

    [Test]
    public void Order(){

        var Original = new List<CustomerDTO>(){
            new CustomerDTO(){
                LastName = "Aaaa",
                FirstName = "Aaaa"
            },
            new CustomerDTO(){
                LastName = "Aaaa",
                FirstName = "Bbbb"
            },
            new CustomerDTO(){
                LastName = "Cccc",
                FirstName = "Aaaa"
            },
            new CustomerDTO(){
                LastName = "Cccc",
                FirstName = "Bbbb"
            },
            new CustomerDTO(){
                LastName = "Dddd",
                FirstName = "Aaaa"
            },
            new CustomerDTO(){
                LastName = "Bbbb",
                FirstName = "Bbbb"
            }
        };

        var Sorted = Sorting.BubbleSort( Original, ValidationHelpers.IsGreater );

        var Expected = new List<CustomerDTO>(){
            new CustomerDTO(){
                LastName = "Aaaa",
                FirstName = "Aaaa",
            },
            new CustomerDTO(){
                LastName = "Aaaa",
                FirstName = "Bbbb",
            },
            new CustomerDTO(){
                LastName = "Bbbb",
                FirstName = "Bbbb",
            },
            new CustomerDTO(){
                LastName = "Cccc",
                FirstName = "Aaaa",
            },
            new CustomerDTO(){
                LastName = "Cccc",
                FirstName = "Bbbb",
            },
            new CustomerDTO(){
                LastName = "Dddd",
                FirstName = "Aaaa",
            }
        };

        Assert.That( Expected, Has.Count.EqualTo( Sorted.Count ) );

        for( var Index = 0; Index < Expected.Count ; Index++ ){

            Assert.That( Sorted[Index].FirstName, Is.EqualTo( Expected[Index].FirstName ), $"FirstName mismatch at index {Index}" );
            Assert.That( Sorted[Index].LastName, Is.EqualTo( Expected[Index].LastName ), $"LastName mismatch at index {Index}" );

        }

    }

}
