using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PickMeApp.Application.Interfaces;
using PickMeApp.Application.Models.ChatDtos;
using PickMeApp.Core.Models.Message;
using PickMeApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickMeApp.Web.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private static List<UserChatViewModel> _ActiveUsers = new List<UserChatViewModel>();
        private readonly IChatRepository _chatRepository;

        public ChatHub(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task SendMessage(MessageDto request)
        {
            string userId = IdentityName;
            Guid currentChatId = _ActiveUsers
                    .Where(u => u.Id == request.ReceiverId)
                    .Select(u => u.CurrentChat)
                    .FirstOrDefault();

            // If the chat is not open to the receiving user the message.
            bool isChatActive = currentChatId == request.ChatId;
            Message messageFromRepo = await _chatRepository.CreateMessageAsync(request.ChatId, request.Text, userId, isChatActive);
            if (messageFromRepo == null)
            {
                await Clients.Caller.SendAsync("onError", "SendMessage: Chat with given id does not exist.");
            }

            if (!isChatActive)
            {
                await Clients.Users(request.ReceiverId)
                    .SendAsync("ReceiveOtherChatMessage", new
                    {
                        ChatId = request.ChatId
                    });
            }
            else
            {
                await Clients.Users(request.ReceiverId)
                    .SendAsync("ReceiveMessage", new
                    {
                        Text = messageFromRepo.Text,
                        SenderId = userId,
                        Timestamp = messageFromRepo.Timestamp.ToString("dd/MM/yyyy hh:mm:ss")
                    });
            }
        }

        public async Task OpenChat(OpenChatDto request)
        {
            try
            {
                var userViewModel = _ActiveUsers.FirstOrDefault(u => u.Id == IdentityName);
                if (userViewModel == null)
                {
                    userViewModel = new UserChatViewModel(IdentityName, request.ChatId);
                    _ActiveUsers.Add(userViewModel);
                }
                else
                {
                    _ActiveUsers.Remove(userViewModel);
                    _ActiveUsers.Add(new UserChatViewModel(IdentityName, request.ChatId));
                }

                if (request.HasUnreadedMessages)
                {
                    await _chatRepository.CleanUnreadedMessagesCounterAsync(request.ChatId, IdentityName);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "OpenChat:" + ex.Message);
            }
            return;
        }

        public override Task OnConnectedAsync()
        {
            try
            {
                if (!_ActiveUsers.Any(u => u.Id == IdentityName))
                {
                    var userViewModel = new UserChatViewModel(IdentityName);
                    _ActiveUsers.Add(userViewModel);
                }
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnConnected:" + ex.Message);
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var user = _ActiveUsers.Where(u => u.Id == IdentityName).First();
                _ActiveUsers.Remove(user);
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
            }

            return base.OnDisconnectedAsync(exception);
        }

        private string IdentityName
        {
            get { return Context.User.Identity.Name; }
        }
    }
}
