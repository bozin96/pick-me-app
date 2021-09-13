using System;
using System.Collections.Generic;
using System.Text;

namespace PickMeApp.Core.Models.Notification
{
    public abstract class NotificationPayload
    {
        public string Header { get; set; }

        public string Body { get; set; }
    }
}
