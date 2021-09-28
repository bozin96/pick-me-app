using System;

namespace PickMeApp.Core.Models.Notification
{
    public class RideRequestNotificationPayload
    {
        public float StartLongitude { get; set; }

        public float StartLatitude { get; set; }

        public float EndLongitude { get; set; }

        public float EndLatitude { get; set; }

        public string StartWaypoint { get; set; }

        public string EndWaypoint { get; set; }

        public int NumberOfPassengers { get; set; }

        public DateTime StartDate { get; set; }

        public string PassengerId { get; set; }

        public string UserFullName { get; set; }

        public void AddUserInfo(ApplicationUser user)
        {
            PassengerId = user.Id;
            UserFullName = $"{user.FirstName} {user.LastName}";
        }
    }
}
