using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COVERater.API.Models;
using COVERater.API.Dto;

namespace COVERater.API.Services
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool ValidMappingExistsFor<T, T1>(string bindingOrderBy);
    }

    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _authPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
                {"MainCategory", new PropertyMappingValue(new List<string>() {"MainCategory"})},
                {"Age", new PropertyMappingValue(new List<string>() {"Age"})},
                {"Name", new PropertyMappingValue(new List<string>() {"Name"})},
                {"ShortCode", new PropertyMappingValue(new List<string>() {"ShortCode"})}
            };

        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<UserDto, User>(_authPropertyMapping));
            _propertyMappings.Add(new PropertyMapping<UsersGuessDto, UsersGuess>(_authPropertyMapping));
            _propertyMappings.Add(new PropertyMapping<ImageDto, Image>(_authPropertyMapping));
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrEmpty(fields)) return true;

            var fieldsAfterSplit = fields.Split(',');

            foreach (var fieldAfterSplit in fieldsAfterSplit)
            {
                var trimmedField = fieldAfterSplit.Trim();
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ? trimmedField : trimmedField.Remove(indexOfFirstSpace);

                if (!propertyName.Contains(propertyName)) return false;

            }

            return true;
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            //Get matching mapping
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();
            if (matchingMapping.Any())
            {
                return matchingMapping.First()._MappingDictionary;
            }
            throw  new Exception($"Cannot find exact property mapping instance "+ $"for<{typeof(TSource)},{typeof(TDestination)}>");
        }
    }
}
