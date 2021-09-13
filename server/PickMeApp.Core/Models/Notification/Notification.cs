using System;
using System.Collections.Generic;
using System.Text;

namespace PickMeApp.Core.Models.Notification
{
    public abstract class Notification<T> where T : NotificationPayload
    {
        NotificationType NotificationType { get; set; }

        public ApplicationUser UserTo { get; set; }
        public string UserToId { get; set; }

        public ApplicationUser UserFrom { get; set; }
        public string UserFromId { get; set; }

        public T Payload { get; set; }
    }
}
