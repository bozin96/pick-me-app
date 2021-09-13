using PickMeApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PickMeApp.Application.Models.RideDtos
{
    public class RideDto
    {
        public Guid Id { get; set; }

        public DateTime StartDate { get; set; }

        public List<RouteLeg> RouteLegs { get; set; }

        public List<Waypoint> Waypoints { get; set; }

        public int NumberOfPassengers { get; set; }

        public int NumberOfFreeSeats { get; set; }

        public bool PetFriendly { get; set; }


    }
}
