using System;

namespace PickMeApp.Core.Models
{
    public class PassengerOnRide : BaseEntity
    {
        public Guid RideId { get; set; }

        public Ride Ride { get; set; }

        public string PassengerId { get; set; }

        public ApplicationUser Passenger { get; set; }

        public int? Review { get; set; }

        public string StartWaypoint { get; set; }

        public string EndWaypoint { get; set; }

        public string DriverName { get; set; }

        public DateTime StartDate { get; set; }
    }
}
