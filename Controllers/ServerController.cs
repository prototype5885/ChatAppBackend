using ChatAppBackend.Data;
using ChatAppBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackend.Controllers;

[ApiController]
[Route("Api/[controller]")]
[Authorize]
public class ServerController(
    ApplicationDbContext db,
    UserManager<RegisteredUser> userManager
) : Controller
{
    [HttpPost("Post")]
    public async Task<IActionResult> Post(AddServer data)
    {
        var user = await userManager.GetUserAsync(User);

        var server = new Server
        {
            Id = Snowflake.New(),
            OwnerId = user.Id,
            Name = data.Name,
            Picture = ""
        };


        await db.Servers.AddAsync(server);
        await db.SaveChangesAsync();

        return Ok(server);
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAll()
    {
        var servers = await db.Servers.ToListAsync();
        // Thread.Sleep(1000);
        return Ok(servers);
    }
}