using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data.Attributes
{
    public class NotificationHub : Hub
    {
        public async Task SendMessages(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public void SendMessage(string groupid, string message)
        {
            Clients.Group(groupid).SendAsync("ReceiveMessage", groupid, message);
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