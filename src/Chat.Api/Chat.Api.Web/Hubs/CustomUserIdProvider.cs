using System.Linq;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Web.Hubs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            var id = connection.GetHttpContext().Request.Headers["x-user-id"].FirstOrDefault();
            return id;
            //return connection.User?.FindFirst("sub")?.Value;
        }
    }
}