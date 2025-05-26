using Microsoft.AspNetCore.SignalR;

namespace ChatAppBackend.Hubs;

public class ChatHub : Hub
{
    public async Task Test(string message)
    {
        await Clients.All.SendAsync("Test", message);
    }
}