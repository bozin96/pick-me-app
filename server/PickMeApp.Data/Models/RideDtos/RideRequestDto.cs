using System;

namespace PickMeApp.Application.Models.RideDtos
{
    public class RideRequestDto
    {
        public float StartLongitude { get; set; }

        public float StartLatitude { get; set; }

        public float EndLongitude { get; set; }

        public float EndLatitude { get; set; }

        public string StartWaypoint { get; set; }

        public string EndWaypoint { get; set; }

        public int NumberOfPassengers { get; set; }

        public DateTime StartDate { get; set; }
    }
}
