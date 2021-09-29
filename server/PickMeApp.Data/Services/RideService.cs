using PickMeApp.Application.Helpers;
using PickMeApp.Application.Interfaces;
using PickMeApp.Application.Models.RideDtos;
using PickMeApp.Core.Models;
using System;
using System.Collections.Generic;

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
                var startLong = Math.Round(leg.Longitude, 1);
                var startLat = Math.Round(leg.Latitude, 1);

                ride.QueryField += $"{startLong}:{startLat};";
            }

            ride.NumberOfFreeSeats = ride.NumberOfPassengers;
            ride.RouteLegs.ForEach(l => l.NumberOfFreeSpaces = ride.NumberOfPassengers);

            ride.StartDate = ride.StartDate.ToUniversalTime();

            return ride;
        }

        public IEnumerable<RideDto> CheckFreeSeats(IEnumerable<RideDto> rideDtos, RideResourceParameters resourceParameters)
        {
            foreach (var ride in rideDtos)
            {
                var drivePrice = 0.0f;
                var driveTime = 0;
                var minFreeSeats = ride.NumberOfPassengers;
                var firstWaypointFound = false;
                foreach (var routeLeg in ride.RouteLegs)
                {
                    // If start waypoint was found.
                    if (!firstWaypointFound &&
                        (resourceParameters.StartLongitude - 0.1) <= routeLeg.StartLongitude && routeLeg.StartLongitude <= (resourceParameters.StartLongitude + 0.1) &&
                        (resourceParameters.StartLatitude - 0.1) <= routeLeg.StartLatitude && routeLeg.StartLatitude <= (resourceParameters.StartLatitude + 0.1))
                    {
                        firstWaypointFound = true;
                    }
                    // If waypoint is between start and end waypoint.
                    if (firstWaypointFound)
                    {
                        if (routeLeg.NumberOfFreeSpaces < resourceParameters.NumberOfPassengers)
                        {
                            ride.NumberFreeSeats = 0;
                            break;
                        }
                        drivePrice += routeLeg.Price;
                        driveTime += routeLeg.Time;
                        minFreeSeats = routeLeg.NumberOfFreeSpaces < minFreeSeats ? routeLeg.NumberOfFreeSpaces : minFreeSeats;
                    }

                    // If end waypoint was found.
                    if ((resourceParameters.EndLongitude - 0.1) <= routeLeg.EndLongitude && routeLeg.EndLongitude <= (resourceParameters.EndLongitude + 0.1) &&
                       (resourceParameters.EndLatitude - 0.1) <= routeLeg.EndLatitude && routeLeg.EndLatitude <= (resourceParameters.EndLatitude + 0.1))
                    {
                        ride.Price = drivePrice;
                        ride.Time = driveTime;
                        ride.NumberFreeSeats = routeLeg.NumberOfFreeSpaces >= resourceParameters.NumberOfPassengers ? minFreeSeats : 0;
                        break;
                    }
                }
            }
            return rideDtos;
        }

        public bool HasFreeSeats(Ride ride, RideRequestDto rideRequest)
        {
            var firstWaypointFound = false;
            foreach (var routeLeg in ride.RouteLegs)
            {
                // If start waypoint was found.
                if (!firstWaypointFound &&
                    (rideRequest.StartLongitude - 0.1) <= routeLeg.StartLongitude && routeLeg.StartLongitude <= (rideRequest.StartLongitude + 0.1) &&
                    (rideRequest.StartLatitude - 0.1) <= routeLeg.StartLatitude && routeLeg.StartLatitude <= (rideRequest.StartLatitude + 0.1))
                {
                    firstWaypointFound = true;
                }

                // If waypoint is between start and end waypoint.
                if (firstWaypointFound)
                {
                    if (routeLeg.NumberOfFreeSpaces < rideRequest.NumberOfPlaces)
                    {
                        return false;
                    }
                }

                // If end waypoint was found.
                if ((rideRequest.EndLongitude - 0.1) <= routeLeg.EndLongitude && routeLeg.EndLongitude <= (rideRequest.EndLongitude + 0.1) &&
                    (rideRequest.EndLatitude - 0.1) <= routeLeg.EndLatitude && routeLeg.EndLatitude <= (rideRequest.EndLatitude + 0.1))
                {
                    return routeLeg.NumberOfFreeSpaces >= rideRequest.NumberOfPlaces;
                }
            }
            return false;
        }

        public bool HasPassengers(Ride ride)
        {
            foreach (var routeLeg in ride.RouteLegs)
            {
                if (routeLeg.NumberOfFreeSpaces < ride.NumberOfPassengers)
                {
                    return true;
                }
            }
            return false;
        }

        public (StatusErrorDto, Ride) AcceptRideRequest(Ride ride, RideRequestDto rideRequest)
        {
            var firstWaypointFound = false;
            var endWaypointFound = false;
            List<RouteLeg> newRouteLegs = new List<RouteLeg>();
            foreach (var routeLeg in ride.RouteLegs)
            {
                // If start waypoint was found.
                if (!firstWaypointFound &&
                    (rideRequest.StartLongitude - 0.1) <= routeLeg.StartLongitude && routeLeg.StartLongitude <= (rideRequest.StartLongitude + 0.1) &&
                    (rideRequest.StartLatitude - 0.1) <= routeLeg.StartLatitude && routeLeg.StartLatitude <= (rideRequest.StartLatitude + 0.1))
                {
                    firstWaypointFound = true;
                }

                // If waypoint is between start and end waypoint.
                if (firstWaypointFound && !endWaypointFound)
                {
                    if (routeLeg.NumberOfFreeSpaces < rideRequest.NumberOfPlaces)
                    {
                        return (new StatusErrorDto("There is no free seats.", 409), ride);
                    }
                    routeLeg.NumberOfFreeSpaces -= rideRequest.NumberOfPlaces;
                }

                // If end waypoint was found.
                if ((rideRequest.EndLongitude - 0.1) <= routeLeg.EndLongitude && routeLeg.EndLongitude <= (rideRequest.EndLongitude + 0.1) &&
                    (rideRequest.EndLatitude - 0.1) <= routeLeg.EndLatitude && routeLeg.EndLatitude <= (rideRequest.EndLatitude + 0.1))
                {
                    endWaypointFound = true;
                }

                newRouteLegs.Add(routeLeg);
            }

            if (!firstWaypointFound && !endWaypointFound)
            {
                return (new StatusErrorDto("Waypoints were not found. ", 409), ride);
            }

            ride.RouteLegs = newRouteLegs;
            return (new StatusErrorDto(), ride);
        }
    }
}
