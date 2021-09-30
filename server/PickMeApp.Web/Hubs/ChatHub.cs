using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PickMeApp.Application.Interfaces;
using PickMeApp.Application.Models.ChatDtos;
using PickMeApp.Core.Models.Message;
using PickMeApp.Web.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickMeApp.Web.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, List<ChatViewModel>> _ActiveChats = new ConcurrentDictionary<string, List<ChatViewModel>>();
        private readonly IChatRepository _chatRepository;

        public ChatHub(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task SendMessage(MessageDto request)
        {
            // Check if the chat is not open to the user to whom the message is being sent.
            bool isChatActive = true;
            List<string> connections = new List<string>();

            _ActiveChats.TryGetValue(request.ReceiverId, out List<ChatViewModel> chats);
            // User is not active
            if (chats == null)
            {
                isChatActive = false;
            }
            else
                connections = chats
                    .Where(c => c.CurrentChat == request.ChatId)
                    .Select(c => c.ConnectionId)
                    .ToList();

            // User is active but chat is not open, send notification.
            if (connections.Count == 0)
            {
                isChatActive = false;
                await Clients.Users(request.ReceiverId)
                    .SendAsync("ReceiveOtherChatMessage", new
                    {
                        ChatId = request.ChatId
                    });
            }

            Message messageFromRepo = await _chatRepository.CreateMessageAsync(request.ChatId, request.Text, IdentityName, isChatActive);
            if (messageFromRepo == null)
            {
                await Clients.Caller.SendAsync("onError", "SendMessage: Chat with given id does not exist.");
            }

            // If chat is active send message.
            if (isChatActive)
                await Clients.Clients(connections)
                    .SendAsync("ReceiveMessage", new
                    {
                        Text = messageFromRepo.Text,
                        SenderId = IdentityName,
                        Timestamp = messageFromRepo.Timestamp
                    });
        }

        // kad menja setuje novi umesto starog
        public async Task OpenChat(OpenChatDto request)
        {
            try
            {
                _ActiveChats.TryGetValue(IdentityName, out List<ChatViewModel> chats);
                if (chats != null)
                {
                    // remove old chat for connection and add new.
                    chats.RemoveAll(c => c.ConnectionId == Context.ConnectionId);
                    chats.Add(new ChatViewModel(Context.ConnectionId, request.ChatId));
                }
                else
                {
                    chats = new List<ChatViewModel>() { new ChatViewModel(Context.ConnectionId, request.ChatId) };
                }
                _ActiveChats.AddOrUpdate(IdentityName, chats, (IdentityName, chats) => chats);

                if (request.HasUnreadedMessages)
                    await _chatRepository.CleanUnreadedMessagesCounterAsync(request.ChatId, IdentityName);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "OpenChat:" + ex.Message);
            }
            return;
        }

        // samo kad menja page
        public void CloseChat()
        {
            _ActiveChats.TryGetValue(IdentityName, out List<ChatViewModel> chats);
            if (chats != null)
            {
                chats.RemoveAll(c => c.ConnectionId == Context.ConnectionId);
                _ActiveChats.AddOrUpdate(IdentityName, chats, (IdentityName, chats) => chats);
            }
        }

        public override Task OnConnectedAsync()
        {
            try
            {
                _ActiveChats.TryAdd(IdentityName, new List<ChatViewModel>());
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
                _ActiveChats.TryGetValue(IdentityName, out List<ChatViewModel> chats);
                if (chats != null)
                {
                    chats.RemoveAll(c => c.ConnectionId == Context.ConnectionId);

                    _ActiveChats.Remove(IdentityName, out List<ChatViewModel> removedChats);
                    if (chats.Count != 0)
                        _ActiveChats.TryAdd(IdentityName, chats);
                }
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
