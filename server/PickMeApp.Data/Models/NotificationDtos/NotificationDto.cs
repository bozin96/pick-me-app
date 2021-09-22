using PickMeApp.Core.Models.Notification;
using System;
using System.Collections.Generic;
using System.Text;

namespace PickMeApp.Application.Models.NotificationDtos
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }

        public string UserToId { get; set; }

        public string UserFromId { get; set; }

        public Guid RideId { get; set; }

        public string Body { get; set; }

        public string Header { get; set; }

        public string UserFromName { get; set; }

        public string UserFromImage { get; set; }
    }
}
