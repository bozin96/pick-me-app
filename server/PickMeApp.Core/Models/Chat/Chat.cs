using System;
using System.Collections.Generic;

namespace PickMeApp.Core.Models.Message
{
    public class Chat : BaseEntity
    {
        public Chat()
        {

        }

        public Chat(string firstUserId, string secondUserId)
        {
            FirstUserId = firstUserId;
            SecondUserId = secondUserId;
        }

        public ApplicationUser FirstUser { get; set; }
        public string FirstUserId { get; set; }

        public ApplicationUser SecondUser { get; set; }
        public string SecondUserId { get; set; }

        public List<Message> Messages { get; set; }
        public DateTime LastMessageTimeStamp { get; set; }
        public string LastMessageSenderId { get; set; }
        public int NumberOfUnreadedMessages { get; set; }
    }
}