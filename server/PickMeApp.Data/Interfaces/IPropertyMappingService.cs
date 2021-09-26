using PickMeApp.Application.Helpers;
using System.Collections.Generic;

namespace PickMeApp.Application.Interfaces
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool ValidMappingExistsFor<TSource, TDestination>(string fields);
    }
}
