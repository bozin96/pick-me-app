using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PickMeApp.Core.Models
{
    public class Ride : BaseEntity
    {
        public string QueryField { get; set; } // list of places separated by '|'

        public DateTime StartDate { get; set; }

        public List<RouteLeg> RouteLegs { get; set; }

        public List<Waypoint> Waypoints { get; set; }

        public int RouteIndex { get; set; }

        public int NumberOfPassengers { get; set; }

        public int NumberOfFreeSeats { get; set; }

        public bool PetFriendly { get; set; }

        public string DriverId { get; set; }

        public ApplicationUser Driver { get; set; }

        public List<PassengerOnRide> Passengers { get; set; }
    }
}
