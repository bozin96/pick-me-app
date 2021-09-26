using PickMeApp.Core.Models;
using System;
using System.Collections.Generic;

namespace PickMeApp.Application.Models.RideDtos
{
    public class RideDto
    {
        public Guid Id { get; set; }

        public DateTime StartDate { get; set; }

        public List<RouteLeg> RouteLegs { get; set; }

        public List<Waypoint> Waypoints { get; set; }

        public int RouteIndex { get; set; }

        public int NumberOfPassengers { get; set; }

        public bool PetFriendly { get; set; }

        public bool HasFreeSeats { get; set; }

        public string DriverName { get; set; }

        public string DriverId { get; set; }

        public float DriverRate { get; set; }
    }
}
