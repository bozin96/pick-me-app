using PickMeApp.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PickMeApp.Application.Models.RideDtos
{
    public abstract class RideForManipulationDto
    {
        public DateTime StartDate { get; set; }

        public List<Waypoint> Waypoints { get; set; }

        public List<RouteLeg> RouteLegs { get; set; }

        public int NumberOfPassengers { get; set; }

        public bool PetFriendly { get; set; }
    }
}
