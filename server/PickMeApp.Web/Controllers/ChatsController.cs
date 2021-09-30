using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub> _chatContext;

        public ChatsController(
            IChatRepository chatRepository,
            IMapper mapper,
            IHubContext<ChatHub> chatContext)
        {
            _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _chatContext = chatContext ?? throw new ArgumentNullException(nameof(chatContext));

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

            List<ChatDto> chatDtos = _mapper.Map<List<ChatDto>>(chatsFromRepo);
            return Ok(chatDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChatAsync(ChatForCreationDto request)
        {
            if (!ModelState.IsValid)
            {
                return ResponseModelStateErrors();
            }

            var chatDto = new ChatDto();
            var currentUserId = GetUserId();
            var chat = await _chatRepository.GetChatByUsersAsync(currentUserId, request.UserId);
            if (chat == null)
            {
                chat = await _chatRepository.CreateChat(currentUserId, request.UserId);

                chatDto = _mapper.Map<ChatDto>(chat);
                await _chatContext.Clients.User(request.UserId).SendAsync("NewChatRequest", chatDto);
            }
            else
            {
                chatDto = _mapper.Map<ChatDto>(chat);
            }
            return Ok(chatDto);
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
