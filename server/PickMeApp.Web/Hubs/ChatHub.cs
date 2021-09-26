using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PickMeApp.Application.Interfaces;
using PickMeApp.Application.Models.ChatDtos;
using System;
using System.Threading.Tasks;

namespace PickMeApp.Web.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatRepository _chatRepository;
        //private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();

        public ChatHub(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task SendMessage(MessageDto request)
        {
            string userId = IdentityName;
            var messageFromRepo = await _chatRepository.CreateMessageAsync(request.ChatId, request.Text, userId);
            if (messageFromRepo == null)
            {
                // await Clients.OthersInGroup(request.ChatId.ToString())
                await Clients.Users(request.ReceiverId)
                    .SendAsync("ErrorMessage", new
                    {
                        Text = "Chat with given id does not exist",
                        SenderId = userId,
                        Timestamp = messageFromRepo.Timestamp.ToString("dd/MM/yyyy hh:mm:ss")
                    });
            }

            // await Clients.OthersInGroup(request.ChatId.ToString())
            await Clients.Users(request.ReceiverId)
                .SendAsync("ReceiveMessage", new
                {
                    Text = messageFromRepo.Text,
                    SenderId = userId,
                    Timestamp = messageFromRepo.Timestamp.ToString("dd/MM/yyyy hh:mm:ss")
                });
        }

        public Task JoinRoom(string roomId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        public Task LeaveRoom(string roomId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        }

        public override Task OnConnectedAsync()
        {
            //try
            //{
            //    var user = _context.Users.Where(u => u.UserName == IdentityName).FirstOrDefault();
            //    var userViewModel = _mapper.Map<ApplicationUser, UserViewModel>(user);
            //    userViewModel.Device = GetDevice();
            //    userViewModel.CurrentRoom = "";

            //    if (!_Connections.Any(u => u.Username == IdentityName))
            //    {
            //        _Connections.Add(userViewModel);
            //        _ConnectionsMap.Add(IdentityName, Context.ConnectionId);
            //    }

            //    Clients.Caller.SendAsync("getProfileInfo", user.FullName, user.Avatar);
            //}
            //catch (Exception ex)
            //{
            //    Clients.Caller.SendAsync("onError", "OnConnected:" + ex.Message);
            //}
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            //try
            //{
            //    var user = _Connections.Where(u => u.Username == IdentityName).First();
            //    _Connections.Remove(user);

            //    // Tell other users to remove you from their list
            //    Clients.OthersInGroup(user.CurrentRoom).SendAsync("removeUser", user);

            //    // Remove mapping
            //    _ConnectionsMap.Remove(user.Username);
            //}
            //catch (Exception ex)
            //{
            //    Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
            //}

            return base.OnDisconnectedAsync(exception);
        }


        private string IdentityName
        {
            get { return Context.User.Identity.Name; }
        }
    }
}
