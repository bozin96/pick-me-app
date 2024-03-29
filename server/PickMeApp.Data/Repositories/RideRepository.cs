﻿using iComplyICO.Data;
using Microsoft.EntityFrameworkCore;
using PickMeApp.Application.Extensions;
using PickMeApp.Application.Helpers;
using PickMeApp.Application.Interfaces;
using PickMeApp.Application.Models.RideDtos;
using PickMeApp.Core.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PickMeApp.Application.Repositories
{
    public class RideRepository : GenericRepository<Ride>, IRideRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;

        public RideRepository(ApplicationDbContext context, IPropertyMappingService propertyMappingService) : base(context)
        {
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }

        public async Task<PagedList<Ride>> ListAsync(RideResourceParameters resourceParameters, string driverId = "")
        {
            if (resourceParameters == null)
            {
                throw new ArgumentNullException(nameof(resourceParameters));
            }

            var collection = _entities as IQueryable<Ride>;

            if (resourceParameters.DateTime != new DateTime())
            {
                collection = collection.Where(e => e.StartDate.Date == resourceParameters.DateTime.ToUniversalTime().Date);
            }
            else
            {
                collection = collection.Where(e => e.StartDate > DateTime.UtcNow);
            }

            if (resourceParameters.StartLongitude != 0 &&
                resourceParameters.StartLatitude != 0 &&
                resourceParameters.EndLongitude != 0 &&
                resourceParameters.EndLatitude != 0)
            {
                string startLongitude = Regex.Escape(Math.Round(resourceParameters.StartLongitude, 1).ToString());
                string startLongitudeMinus = Regex.Escape(Math.Round(resourceParameters.StartLongitude - 0.1f, 1).ToString());
                string startLongitudePlus = Regex.Escape(Math.Round(resourceParameters.StartLongitude + 0.1f, 1).ToString());

                string startLatitude = Regex.Escape(Math.Round(resourceParameters.StartLatitude, 1).ToString());
                string startLatitudeMinus = Regex.Escape(Math.Round(resourceParameters.StartLatitude - 0.1f, 1).ToString());
                string startLatitudePlus = Regex.Escape(Math.Round(resourceParameters.StartLatitude + 0.1f, 1).ToString());

                string endLongitude = Regex.Escape(Math.Round(resourceParameters.EndLongitude, 1).ToString());
                string endLongitudeMinus = Regex.Escape(Math.Round(resourceParameters.EndLongitude - 0.1f, 1).ToString());
                string endLongitudePlus = Regex.Escape(Math.Round(resourceParameters.EndLongitude + 0.1f, 1).ToString());

                string endLatitude = Regex.Escape(Math.Round(resourceParameters.EndLatitude, 1).ToString());
                string endLatitudeMinus = Regex.Escape(Math.Round(resourceParameters.EndLatitude - 0.1f, 1).ToString());
                string endLatitudePlus = Regex.Escape(Math.Round(resourceParameters.EndLatitude + 0.1f, 1).ToString());

                string regexPattern =
                    $"({startLongitude}|{startLongitudeMinus}|{startLongitudePlus}):({startLatitude}|{startLatitudeMinus}|{startLatitudePlus})" +
                    $"(.*)" +
                    $"({endLongitude}|{endLongitudeMinus}|{endLongitudeMinus}):({endLatitude}|{endLatitudeMinus}|{endLatitudePlus})";

                collection = collection.Where(e => Regex.IsMatch(e.QueryField, regexPattern));
            }

            if(!string.IsNullOrEmpty(driverId))
            {
                collection = collection.Where(e => e.DriverId != driverId);
            }

            collection = collection.Include(e => e.Driver);

            if (!string.IsNullOrWhiteSpace(resourceParameters.OrderBy))
            {
                // get property mapping dictionary
                var authorPropertyMappingDictionary =
                    _propertyMappingService.GetPropertyMapping<RideDto, Ride>();

                collection = collection.ApplySort(resourceParameters.OrderBy, authorPropertyMappingDictionary);
            }

            return await PagedList<Ride>.CreateAsync(collection,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }

        public async Task<PagedList<Ride>> ListAsync(MyRideResourceParameters resourceParameters)
        {
            if (resourceParameters == null)
            {
                throw new ArgumentNullException(nameof(resourceParameters));
            }

            var collection = _entities as IQueryable<Ride>;

            if (resourceParameters.DateTime != new DateTime())
            {
                collection = collection.Where(e =>
                    e.StartDate.Day == resourceParameters.DateTime.Day &&
                    e.StartDate.Month == resourceParameters.DateTime.Month &&
                    e.StartDate.Year == resourceParameters.DateTime.Year);
            }

            if (!string.IsNullOrEmpty(resourceParameters.DriverId))
            {
                collection = collection.Where(e => e.DriverId == resourceParameters.DriverId);
            }

            if (!string.IsNullOrWhiteSpace(resourceParameters.OrderBy))
            {
                // get property mapping dictionary
                var authorPropertyMappingDictionary =
                    _propertyMappingService.GetPropertyMapping<RideDto, Ride>();

                collection = collection.ApplySort(resourceParameters.OrderBy, authorPropertyMappingDictionary);
            }

            return await PagedList<Ride>.CreateAsync(collection,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }
    }
}
