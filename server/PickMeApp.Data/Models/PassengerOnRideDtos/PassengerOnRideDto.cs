using System;

namespace PickMeApp.Application.Models.PassengerOnRideDtos
{
    public class PassengerOnRideDto
    {
        public Guid Id { get; set; }

        public Guid RideId { get; set; }

        public string PassengerId { get; set; }

        public int? Review { get; set; }

        public string StartWaypoint { get; set; }

        public string EndWaypoint { get; set; }

        public string DriverName { get; set; }

        public DateTime StartDate { get; set; }
    }
}
