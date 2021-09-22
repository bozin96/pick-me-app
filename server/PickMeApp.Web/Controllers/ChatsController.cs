using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PickMeApp.Application.Helpers;
using PickMeApp.Application.Interfaces;
using PickMeApp.Application.Models.ChatDtos;
using PickMeApp.Core.Constants;
using PickMeApp.Web.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickMeApp.Web.Controllers
{
    [Route("api/chats")]
    [ApiController]
    public class ChatsController : ApiController
    {
        private readonly IChatRepository _chatRepository;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatsController(
            IChatRepository chatRepository,
            IHubContext<ChatHub> hubContext)
        {
            _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        }

        [HttpGet]
        public async Task<IActionResult> GetChatsAsync([FromQuery] ResourceParameters resourceParameters)
        {
            var currentUserId = GetUserId();
            var chatsFromRepo = await _chatRepository.GetChatsAsync(currentUserId, resourceParameters);

            var paginationMetadata = new
            {
                totalCount = chatsFromRepo.TotalCount,
                pageSize = chatsFromRepo.PageSize,
                currentPage = chatsFromRepo.CurrentPage,
                totalPages = chatsFromRepo.TotalPages
            };
            Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

            return Ok(chatsFromRepo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChatAsync(ChatForCreationDto request)
        {
            var currentUserId = GetUserId();
            if (await _chatRepository.ChatExistsAsync(request.UserId, currentUserId))
                return ReturnError(StatusCodes.Status409Conflict, "Chat already exist for these two users");

            var chat = await _chatRepository.CreateChat(currentUserId, request.UserId);

            return Ok(chat);
        }

        [HttpGet("{chatId}/messages")]
        public async Task<IActionResult> GetChatMessageAsync(Guid chatId, [FromQuery] ResourceParameters resourceParameters)
        {
            var messagesFromRepo = await _chatRepository.GetChatMessagesAsync(chatId, resourceParameters);

            var paginationMetadata = new
            {
                totalCount = messagesFromRepo.TotalCount,
                pageSize = messagesFromRepo.PageSize,
                currentPage = messagesFromRepo.CurrentPage,
                totalPages = messagesFromRepo.TotalPages
            };
            Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

            return Ok(messagesFromRepo);
        }

        [HttpPost("{chatId}/messages")]
        public async Task<IActionResult> SendMessage(Guid chatId, MessageDto request)
        {
            var userId = GetUserId();
            var message = await _chatRepository.CreateMessageAsync(chatId, request.Text, userId);

            await _hubContext.Clients.Group(chatId.ToString())
                .SendAsync("RecieveMessage", new
                {
                    Text = message.Text,
                    SenderId = userId,
                    Timestamp = message.Timestamp.ToString("dd/MM/yyyy hh:mm:ss")
                });

            return Ok();
        }

        #region Helpers

        [NonAction]
        private string GetUserId()
        {
            return HttpContext.User.Claims
                .Where(c => c.Type == CustomClaimTypes.Id)
                .Select(c => c.Value)
                .FirstOrDefault();
        }

        #endregion
    }
}
