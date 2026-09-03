using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectAPI.Data;
using ProjectAPI.DTOs;
using ProjectAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CtmsDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(CtmsDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            var contact = await _context.Contact
                .FirstOrDefaultAsync(c => c.Email == dto.Email);

            if (contact == null)
                return Unauthorized("Invalid email or password");

            var customer = await _context.Customer.FirstOrDefaultAsync(c => c.ContactId == contact.ContactId);
            var courier = await _context.Courier.FirstOrDefaultAsync(c => c.ContactId == contact.ContactId);
            var admin = await _context.Admin.FirstOrDefaultAsync(a => a.ContactId == contact.ContactId);

            string? storedHash = customer?.PasswordHash ?? courier?.PasswordHash ?? admin?.PasswordHash;
            string role = customer != null ? "Customer" 
                        : courier != null ? "Courier" 
                        : admin != null ? "Admin" : "";

            if (string.IsNullOrEmpty(role) || storedHash != dto.Password)
                return Unauthorized("Invalid email or password");

            int roleId = customer?.CustomerId ?? courier?.CourierId ?? admin?.AdminId ?? 0;

            var token = GenerateJwtToken(contact.Email, role, contact.ContactId, roleId);

            return Ok(new AuthResponseDto
            {
                Token = token,
                Role = role,
                UserId = contact.ContactId,
                RoleId = roleId
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
        {   

            if (string.IsNullOrWhiteSpace(dto.Name))
        return BadRequest("Full Name is required");

    if (string.IsNullOrWhiteSpace(dto.Email))
        return BadRequest("Email is required");

    if (string.IsNullOrWhiteSpace(dto.Password))
        return BadRequest("Password is required");

    if (string.IsNullOrWhiteSpace(dto.Phone))
        return BadRequest("Phone Number is required");

            var existingContact = await _context.Contact.FirstOrDefaultAsync(c => c.Email == dto.Email);
            if (existingContact != null)
                return BadRequest("Email already registered");

            if (!long.TryParse(dto.Phone, out long phoneValue))
    {
        return BadRequest("Invalid phone number format. Use digits only (e.g., 923001234567)");
    }

            var contact = new Contact
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = phoneValue,
                Address = dto.Address
            };

            _context.Contact.Add(contact);
            await _context.SaveChangesAsync();

            string passwordHash = dto.Password;
            int roleId = 0;

            switch (dto.Role.ToLower())
            {
                case "customer":
    var customer = new Customer { ContactId = contact.ContactId, PasswordHash = passwordHash };
    _context.Customer.Add(customer);
    await _context.SaveChangesAsync();
    roleId = customer.CustomerId;  // ← lowercase Id
    break;

case "courier":
    var courier = new Courier { ContactId = contact.ContactId, PasswordHash = passwordHash };
    _context.Courier.Add(courier);
    await _context.SaveChangesAsync();
    roleId = courier.CourierId;  // ← lowercase Id
    break;

case "admin":
    var admin = new Admin { ContactId = contact.ContactId, PasswordHash = passwordHash };
    _context.Admin.Add(admin);
    await _context.SaveChangesAsync();
    roleId = admin.AdminId;  // ← lowercase Id
    break;

                default:
                    return BadRequest("Invalid role");
            }

            var token = GenerateJwtToken(contact.Email, dto.Role, contact.ContactId, roleId);

            return Ok(new AuthResponseDto
            {
                Token = token,
                Role = dto.Role,
                UserId = contact.ContactId,
                RoleId = roleId
            });
        }

        private string GenerateJwtToken(string email, string role, int contactId, int roleId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim("ContactId", contactId.ToString()),
                new Claim("RoleId", roleId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, roleId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        // Raw SQL Register Demo (plain password - for demo only!)
// Raw SQL Register Demo (Full fields + role selection - plain password)
[HttpPost("demo-register-raw")]
public async Task<IActionResult> DemoRegisterRaw([FromBody] RegisterDto dto)
{
    if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email) ||
        string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(dto.Phone) ||
        string.IsNullOrWhiteSpace(dto.Role))
        return BadRequest("All required fields must be provided");

    var normalizedRole = dto.Role.ToLowerInvariant();
    if (normalizedRole != "customer" && normalizedRole != "courier" && normalizedRole != "admin")
        return BadRequest("Invalid role");

    int contactId = 0;

    using var connection = _context.Database.GetDbConnection();
    try
    {
        await connection.OpenAsync();

        // Step 1: Check duplicate email
        using var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = "SELECT COUNT(1) FROM Contact WHERE Email = @email";
        var emailParam = checkCmd.CreateParameter();
        emailParam.ParameterName = "@email";
        emailParam.Value = dto.Email;
        checkCmd.Parameters.Add(emailParam);

        var count = Convert.ToInt32(await checkCmd.ExecuteScalarAsync());
        if (count > 0)
            return BadRequest("Email already registered");

        // Step 2: Insert into Contact (NO PasswordHash column!)
        using var insertCmd = connection.CreateCommand();
        insertCmd.CommandText = @"
            INSERT INTO Contact (Name, Email, Phone, Address)
            VALUES (@name, @email, @phone, @address);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        var pName = insertCmd.CreateParameter(); pName.ParameterName = "@name"; pName.Value = dto.Name;
        var pEmail = insertCmd.CreateParameter(); pEmail.ParameterName = "@email"; pEmail.Value = dto.Email;
        var pPhone = insertCmd.CreateParameter(); pPhone.ParameterName = "@phone"; pPhone.Value = dto.Phone;
        var pAddress = insertCmd.CreateParameter(); pAddress.ParameterName = "@address"; pAddress.Value = dto.Address ?? (object)DBNull.Value;

        insertCmd.Parameters.AddRange(new[] { pName, pEmail, pPhone, pAddress });

        contactId = Convert.ToInt32(await insertCmd.ExecuteScalarAsync());

        // Step 3: Insert into role table with PasswordHash
        var roleSql = normalizedRole switch
        {
            "customer" => "INSERT INTO Customer (ContactId, PasswordHash) VALUES (@cid, @pwd)",
            "courier" => "INSERT INTO Courier (ContactId, PasswordHash) VALUES (@cid, @pwd)",
            "admin" => "INSERT INTO Admin (ContactId, PasswordHash) VALUES (@cid, @pwd)",
            _ => throw new InvalidOperationException()
        };

        using var roleCmd = connection.CreateCommand();
        roleCmd.CommandText = roleSql;
        var pCid = roleCmd.CreateParameter(); pCid.ParameterName = "@cid"; pCid.Value = contactId;
        var pPwd = roleCmd.CreateParameter(); pPwd.ParameterName = "@pwd"; pPwd.Value = dto.Password;
        roleCmd.Parameters.AddRange(new[] { pCid, pPwd });

        await roleCmd.ExecuteNonQueryAsync();

        return Ok(new
        {
            message = $"Registered as {dto.Role} via Raw SQL!",
            email = dto.Email,
            contactId,
            warning = "Password stored!"
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine("RAW REGISTER ERROR: " + ex.ToString());
        return StatusCode(500, "Registration failed: " + ex.Message);
    }
}// Raw SQL Login Demo (plain password comparison)
// Raw SQL Login Demo (plain password comparison)
[HttpPost("demo-login-raw")]
public async Task<IActionResult> DemoLoginRaw([FromBody] LoginDto dto)
{
    if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
        return BadRequest("Email and password required");

    var sql = @"
        SELECT 
            c.ContactId,
            c.Email,
            ISNULL(cust.PasswordHash, ISNULL(cour.PasswordHash, adm.PasswordHash)) AS PasswordHash,
            CASE 
                WHEN cust.ContactId IS NOT NULL THEN 'Customer'
                WHEN cour.ContactId IS NOT NULL THEN 'Courier'
                WHEN adm.ContactId IS NOT NULL THEN 'Admin'
                ELSE 'Unknown'
            END AS Role
        FROM Contact c
        LEFT JOIN Customer cust ON cust.ContactId = c.ContactId
        LEFT JOIN Courier cour ON cour.ContactId = c.ContactId
        LEFT JOIN Admin adm ON adm.ContactId = c.ContactId
        WHERE c.Email = {0}";

    var contact = await _context.Database
        .SqlQueryRaw<ContactLoginDto>(sql, dto.Email)
        .FirstOrDefaultAsync();

    if (contact == null || contact.Role == "Unknown")
        return Unauthorized("Email not found or no role assigned (Raw SQL check)");

    // Plain text comparison (demo only - insecure!)
    if (contact.PasswordHash != dto.Password)
        return Unauthorized("Incorrect password (Raw SQL check)");

    // Generate token
    var token = GenerateJwtToken(contact.Email, contact.Role, contact.ContactId, 0);

    return Ok(new { 
        message = "Logged in successfully via Raw SQL!",
        token,
        role = contact.Role,
        warning = " Password comparison used - NEVER do this!"
    });

}
}
}


        
    
