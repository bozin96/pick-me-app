using System;
using System.Collections.Generic;
using System.Text;

namespace PickMeApp.Application.Models.ChatDtos
{
    public class OpenChatDto
    {
        public Guid ChatId { get; set; }

        public bool HasUnreadedMessages { get; set; }
    }
}
