using System;
using System.Collections.Generic;
using System.Text;

namespace PickMeApp.Core.Models.Message
{
    public class Message : BaseEntity
    {
        public Chat Chat { get; set; }

        public Guid ChatId { get; set; }

        public string SendUserId { get; set; }

        public ApplicationUser SendUser { get; set; }

        public string Text { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
