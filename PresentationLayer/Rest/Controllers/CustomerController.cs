using System.Threading.Tasks;
using BusinessLayer.Toolkit;
using Microsoft.AspNetCore.Mvc;
using RIACustomers.BusinessLayer.Toolkit;
using RIACustomers.Database.Managers;
using RIACustomers.Rest.Models;

namespace RIACustomers.Rest.Controllers;

[Route("[controller]")]
public class CustomerController : Controller {

    public CustomerMan CustomerManager { get; set; }
    public CustomerHelper Helper { get; set; }

    public CustomerController(CustomerMan CustomerManager, CustomerHelper Helper) {
        
        this.CustomerManager = CustomerManager;
        this.Helper = Helper;

    }

    [HttpPost]
    public async Task<IActionResult> Post(CustomersDTO DTOs) {
        
        if( !ModelState.IsValid )
            return Json(new {
                Success = false,
                Message = "The following errors occurred",
                Errors = ModelState.GetErrors()
            });

        var NewCustomers = await CustomerManager.Create( DTOs.Customers );

        // NewCustomers = Sorting.BubbleSort( NewCustomers, IsGreater );

        return Json(new {
            Success = true,
            Message = "Ok",
            // NewCustomers
        });

    }

    [HttpGet]
    public async Task<IActionResult> Get(){

        var Customers = await Helper.GetSortedCustomers();

        return Json(new {
            Success = true,
            Customers = Customers
        });

    }

    [HttpGet("/Customer/Index")]
    public async Task<IActionResult> Index(){

        ViewData["IsThereAnyRecords"] = (await CustomerManager.GetAll()).Count > 0;

        return View();

    }

    [HttpDelete]
    public async Task<IActionResult> Delete(){

        await CustomerManager.DeleteAll();

        return Json(new {
            Success = true
        });

    }

}
