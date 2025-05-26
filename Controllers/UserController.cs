using ChatAppBackend.Data;
using ChatAppBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppBackend.Controllers;

[ApiController]
[Route("Api/[controller]")]
[Authorize]
public class UserController(
    // ILogger<UserController> log,
    ApplicationDbContext db
) : Controller
{

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("{userId:long}")]
    public async Task<ActionResult<RegisteredUser>> GetUser(long userId)
    {
        var user = await db.Users.FindAsync(userId);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpGet("Test")]
    public IActionResult Test()
    {
        return Ok(User.Identity?.Name);
    }
}