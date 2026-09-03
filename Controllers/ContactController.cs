using Microsoft.AspNetCore.Mvc;
using ProjectAPI.Models;
namespace ProjectAPI.Controllers;

[ApiController]
[Route("api/contacts")]
public class ContactController : ControllerBase
{
    private static List<Contact> contacts = new();

    [HttpPost("create")]
    public IActionResult Create(Contact contact)
    {
        contact.ContactId = contacts.Count + 1;
        contacts.Add(contact);
        return Ok(contact);
    }
}