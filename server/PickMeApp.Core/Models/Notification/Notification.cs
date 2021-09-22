using System;
using System.Collections.Generic;
using System.Text;

namespace PickMeApp.Core.Models.Notification
{
    public class Notification : BaseEntity
    {
        public NotificationType Type { get; set; }

        public bool IsVisible { get; set; } = true;

        public ApplicationUser UserTo { get; set; }
        public string UserToId { get; set; }

        public ApplicationUser UserFrom { get; set; }
        public string UserFromId { get; set; }

        public Ride Ride { get; set; }
        public Guid RideId { get; set; }

        public string Body { get; set; }

        public string Header { get; set; }

        public string Payload { get; set; }
    }

    public static class NotificationConfiguration
    {
        public static string NotificationHubMethod = "notification";

        public static string RequestForRideHeader = "Request for ride";

        public static string ResponseOnRideRequestHeader = "Response on ride request";

        public static string RideReviewHeader = "Ride review";

        public static string RequestForRideChanell = "RequestForRide";

        public static string ResponseOnRideRequestChanell = "ResponseOnRideRequest";

        public static string RideReviewChanell = "RideReview";

        public static string RequestForRideBody(
            string startPoint,
            string endPoint,
            DateTime date,
            int numberOfSeats)
        {
            return $"From {startPoint} to {endPoint} on {date} for {numberOfSeats}";
        }

        public static string RideReviewBody(
            string startWaypoint,
            string EndWaypoint,
            DateTime startDate,
            int review)
        {
            return $"You have been reviewed  for ride {startWaypoint}-{EndWaypoint} " +
                $"on {startDate} with review {review}";
        }

        public static string ResponseOnRideRequestBody(
            string driverFullName,
            string startWaypoint,
            string EndWaypoint,
            DateTime startDate,
            bool accept)
        {
            if (accept)
                return $"Your request form ride {startWaypoint}-{EndWaypoint} " +
                    $"on {startDate} has bee accepted by {driverFullName}";

            return $"Your request form ride {startWaypoint}-{EndWaypoint} " +
                    $"on {startDate} has bee declined by {driverFullName}";
        }

    }
}
