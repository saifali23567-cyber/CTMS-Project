using Microsoft.AspNetCore.Mvc;
using ProjectAPI.Models;

namespace ProjectAPI.Controllers;

[ApiController]
[Route("api/couriers")]
public class CourierController : ControllerBase
{
    private static List<Courier> couriers = new();

    [HttpPost("register")]
    public IActionResult Register(Courier courier)
    {
        courier.CourierId = couriers.Count + 1;
        couriers.Add(courier);
        return Ok(courier);
    }
}