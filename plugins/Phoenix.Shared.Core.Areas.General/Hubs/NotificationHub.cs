using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace server.Hubs
{
    public class NotificationHub : Hub
    {
 
        public async Task SendMessages(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public void SendMessage(string groupid, string message)
        {
            Clients.Group(groupid).SendAsync("ReceiveMessage", groupid,message);
        }

        public Task JoinRoom(string roomName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public Task LeaveRoom(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
    }
}