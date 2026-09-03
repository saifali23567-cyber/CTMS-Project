using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPI.Data;
using ProjectAPI.Models;
using ProjectAPI.DTOs;
namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]  // All endpoints in this controller require Admin role
    public class AdminController : ControllerBase
    {
        private readonly CtmsDbContext _context;

        public AdminController(CtmsDbContext context)
        {
            _context = context;
        }

        // GET: api/Admin/couriers
        // Returns list of all couriers with ID, name, email, phone
        [HttpGet("couriers")]
        public async Task<ActionResult<IEnumerable<object>>> GetCouriers()
        {
            var couriers = await _context.Courier
                .Include(c => c.Contact)
                .Select(c => new
                {
                    courierId = c.CourierId,
                    name = c.Contact.Name,
                    email = c.Contact.Email,
                    phone = c.Contact.Phone.HasValue ? c.Contact.Phone.Value.ToString() : "Not provided"
                })
                .OrderBy(c => c.name)
                .ToListAsync();

            return Ok(couriers);
        }

        // GET: api/Admin/customers
        // Returns list of all customers with ID, name, email, phone, address
        [HttpGet("customers")]
        public async Task<ActionResult<IEnumerable<object>>> GetCustomers()
        {
            var customers = await _context.Customer
                .Include(c => c.Contact)
                .Select(c => new
                {
                    customerId = c.CustomerId,
                    name = c.Contact.Name,
                    email = c.Contact.Email,
                    phone = c.Contact.Phone.HasValue ? c.Contact.Phone.Value.ToString() : "Not provided",
                    address = c.Contact.Address ?? "Not provided"
                })
                .OrderBy(c => c.name)
                .ToListAsync();

            return Ok(customers);
        }

        // PUT: api/Admin/assign/{trackingNumber}/{courierId}
        // Assign a courier to a package
        [HttpPut("assign/{trackingNumber}/{courierId}")]
        public async Task<IActionResult> AssignCourier(string trackingNumber, int courierId)
        {
            var package = await _context.Package
                .FirstOrDefaultAsync(p => p.TrackingNumber == trackingNumber.ToUpper());

            if (package == null)
                return NotFound("Package not found");

            var courier = await _context.Courier
                .Include(c => c.Contact)
                .FirstOrDefaultAsync(c => c.CourierId == courierId);

            if (courier == null)
                return NotFound("Courier not found");

            package.CourierId = courierId;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Courier assigned successfully",
                trackingNumber = package.TrackingNumber,
                courierName = courier.Contact.Name
            });
        }

        [HttpGet("demo-customers-raw")]
[Authorize(Roles = "Admin")]
public async Task<ActionResult<IEnumerable<object>>> DemoCustomersRaw()
{
    var sql = @"
    SELECT 
        cu.CustomerId,
        co.Name,
        co.Email,
        ISNULL(CAST(co.Phone AS VARCHAR(20)), 'Not provided') AS Phone,
        ISNULL(co.Address, 'Not provided') AS Address
    FROM Customer cu
    INNER JOIN Contact co ON cu.ContactId = co.ContactId
    ORDER BY co.Name";

    var results = await _context.CustomerDemoDtos
        .FromSqlRaw(sql)
        .AsNoTracking()
        .ToListAsync();
    
    return Ok(results);
    }

[HttpGet("demo-couriers-raw")]
[Authorize(Roles = "Admin")]
public async Task<ActionResult<IEnumerable<object>>> DemoCouriersRaw()
{
    var sql = @"
    SELECT 
        cr.CourierId,
        co.Name,
        co.Email,
        ISNULL(CAST(co.Phone AS VARCHAR(20)), 'Not provided') AS Phone
    FROM Courier cr
    INNER JOIN Contact co ON cr.ContactId = co.ContactId
    ORDER BY co.Name";

        var results = await _context.CourierDemoDtos
    .FromSqlRaw(sql)
    .AsNoTracking()
    .ToListAsync();

    return Ok(results);
}

// ADMIN: Assign Courier (Raw SQL Version)
[HttpPut("demo-assign-raw/{trackingNumber}/{courierId}")]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> DemoAssignCourierRaw(string trackingNumber, int courierId)
{
    var sql = @"
        UPDATE Package 
        SET CourierId = @p1 
        WHERE TrackingNumber = @p0";

    var rows = await _context.Database.ExecuteSqlRawAsync(sql, trackingNumber.ToUpper(), courierId);

    if (rows == 0)
        return NotFound("Package not found or no change");

    return Ok(new { message = "Courier assigned via raw SQL", trackingNumber, courierId });
}
      
  

    }
}