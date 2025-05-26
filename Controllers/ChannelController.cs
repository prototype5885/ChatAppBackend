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
public class ChannelController(
    ApplicationDbContext db,
    UserManager<RegisteredUser> userManager
) : Controller
{
    [HttpPost("Post")]
    public async Task<IActionResult> Post(AddChannel data)
    {
        var user = await userManager.GetUserAsync(User);
        var channel = new Channel
        {
            Id = Snowflake.New(),
            ServerId = data.ServerId,
            Name = data.Name
        };


        await db.Channels.AddAsync(channel);
        await db.SaveChangesAsync();

        return Ok(channel);
    }

    [HttpGet("All/{serverId}")]
    public async Task<IActionResult> EnteredServer(long serverId)
    {
        var channels = await db.Channels.Where(channel => channel.ServerId == serverId).ToListAsync();
        return Ok(channels);
    }
}