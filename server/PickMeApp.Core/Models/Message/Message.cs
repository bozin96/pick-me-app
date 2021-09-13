using System;
using System.Collections.Generic;
using System.Text;

namespace PickMeApp.Core.Models.Message
{
    public class Message
    {
        public string SendUserId { get; set; }

        public ApplicationUser SendUser { get; set; }

        public string ReceiveUserId { get; set; }

        public ApplicationUser ReceiveUser { get; set; }

        public string Text { get; set; }


    }
}
