using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace PickMeApp.Web.Hubs
{
    [Authorize]
    public class NotificationsHub : Hub { }
}
