using Microsoft.AspNetCore.Mvc;
using ProjectAPI.Models;

namespace ProjectAPI.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private static List<Customer> customers = new();

    [HttpPost("register")]
    public IActionResult Register(Customer customer)
    {
        customer.CustomerId = customers.Count + 1;
        customers.Add(customer);
        return Ok(customer);
    }
}