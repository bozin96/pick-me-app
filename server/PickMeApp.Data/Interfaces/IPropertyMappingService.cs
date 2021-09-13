using PickMeApp.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PickMeApp.Application.Interfaces
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool ValidMappingExistsFor<TSource, TDestination>(string fields);
    }
}
