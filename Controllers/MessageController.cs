using System.Security.Claims;
using ChatAppBackend.Data;
using ChatAppBackend.Hubs;
using ChatAppBackend.Models;
using IdGen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackend.Controllers;

[ApiController]
[Route("Api/[controller]")]
[Authorize]
public class MessageController(
    // ILogger<MessageController> log,
    ApplicationDbContext db,
    IHubContext<ChatHub> signalr,
    UserManager<RegisteredUser> userManager
) : Controller
{
    [HttpPost("Post")]
    [Authorize]
    public async Task Post(AddMessage data)
    {
        var user = await userManager.GetUserAsync(User);

        var message = new Message
        {
            Id = Snowflake.New(),
            UserId = user.Id,
            ChannelId = data.ChannelId,
            Msg = data.Msg
        };

        await db.Messages.AddAsync(message);
        await db.SaveChangesAsync();

        await signalr.Clients.All.SendAsync($"message/{data.ChannelId}", message);

    }

    [HttpGet("All/{channelId:long}")]
    public async Task<IActionResult> EnteredChannel(long channelId)
    {
        var messages = await db.Messages.Where(message => message.ChannelId == channelId).Take(50).ToListAsync();
        return Ok(messages);
    }
}