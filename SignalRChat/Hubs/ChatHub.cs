using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    //https://docs.microsoft.com/zh-cn/aspnet/core/tutorials/signalr?view=aspnetcore-3.1&tabs=visual-studio
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            //分发
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
