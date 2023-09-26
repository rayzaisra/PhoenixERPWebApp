using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Helpers;

namespace server.Hubs
{
    [JwtAuthentication]
    public class HelloHub : Hub
    {
        
        public Task BroadcastHello()
        {
            var name = Glosing.Instance.Username;
            return Clients.All.SendAsync("hello");
        }
    }
}