using PickMeApp.Application.Helpers;
using PickMeApp.Application.Interfaces;
using PickMeApp.Application.Models.NotificationDtos;
using PickMeApp.Application.Models.PassengerOnRideDtos;
using PickMeApp.Application.Models.RideDtos;
using PickMeApp.Core.Models;
using PickMeApp.Core.Models.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PickMeApp.Application.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _ridePropertyMapping =
          new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
          {
              { "Id", new PropertyMappingValue(new List<string>() { "Id" }) },
              { "StartWaypoint", new PropertyMappingValue(new List<string>() { "StartPlace" }) },
              { "EndWaypoint", new PropertyMappingValue(new List<string>() { "EndPlace" }) },
              { "DateTime", new PropertyMappingValue(new List<string>() { "DateTime" }) },
              { "NumberOfPassengers", new PropertyMappingValue(new List<string>() { "NumberOfPassengers" }) },
              { "NumberOfFreePlaces", new PropertyMappingValue(new List<string>() { "NumberOfFreePlaces" }) },
              { "PetFriendly", new PropertyMappingValue(new List<string>() { "PetFriendly" }) }
          };

        private Dictionary<string, PropertyMappingValue> _passengerOnRideMapping =
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            { "Id", new PropertyMappingValue(new List<string>() { "Id" }) },
            { "Review", new PropertyMappingValue(new List<string>() { "Review" }) },
            { "StartWaypoint", new PropertyMappingValue(new List<string>() { "StartPlace" }) },
            { "EndWaypoint", new PropertyMappingValue(new List<string>() { "EndPlace" }) },
            { "DriverName", new PropertyMappingValue(new List<string>() { "DriverName" }) },
            { "StartDate", new PropertyMappingValue(new List<string>() { "StartDate" }) }
        };

        private Dictionary<string, PropertyMappingValue> _notificationPropertyMapping =
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            { "Id", new PropertyMappingValue(new List<string>() { "Id" }) },
            { "Type", new PropertyMappingValue(new List<string>() { "Type" }) },
            { "Header", new PropertyMappingValue(new List<string>() { "Header" }) },
        };

        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<RideDto, Ride>(_ridePropertyMapping));
            _propertyMappings.Add(new PropertyMapping<PassengerOnRideDto, PassengerOnRide>(_passengerOnRideMapping));
            _propertyMappings.Add(new PropertyMapping<NotificationDto, Notification>(_notificationPropertyMapping));
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            // the string is separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split(',');

            // run through the fields clauses
            foreach (var field in fieldsAfterSplit)
            {
                // trim
                var trimmedField = field.Trim();

                // remove everything after the first " " - if the fields 
                // are coming from an orderBy string, this part must be 
                // ignored
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                // find the matching property
                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }


        public Dictionary<string, PropertyMappingValue> GetPropertyMapping
           <TSource, TDestination>()
        {
            // get matching mapping
            var matchingMapping = _propertyMappings
                .OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance " +
                $"for <{typeof(TSource)},{typeof(TDestination)}");
        }
    }
}
