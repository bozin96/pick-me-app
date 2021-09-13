using AutoMapper;
using PickMeApp.Application.Helpers;
using PickMeApp.Application.Interfaces;
using PickMeApp.Application.Models.RideDtos;
using PickMeApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PickMeApp.Application.Services
{
    public class RideService : IRideService
    {
        private readonly IRideRepository _rideRepository;


        public RideService(
            IRideRepository rideRepository)
        {
            _rideRepository = rideRepository ?? throw new ArgumentNullException(nameof(rideRepository));

        }

        public Ride PrepareRideForCreation(Ride ride)
        {
            ride.QueryField = string.Empty;
            foreach (var leg in ride.Waypoints)
            {
                var startLong = Math.Round(leg.Longitude,1);
                var startLat = Math.Round(leg.Latitude,1);

                ride.QueryField += $"{startLong}:{startLat};";
            }
            ride.NumberOfFreeSeats = ride.NumberOfPassengers;
            
            return ride;
        }
    }
}
