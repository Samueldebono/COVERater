using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COVERater.API.Services;

namespace COVERater.API.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy,
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (mappingDictionary == null)
            throw new ArgumentNullException(nameof(mappingDictionary));
            if (string.IsNullOrEmpty(orderBy))
                return source;

            var orderByAfterSplit = orderBy.Split(',');

            foreach (var orderAfterSplit in orderByAfterSplit)
            {
                var trimmedOrderbyClause = orderAfterSplit.Trim();

                var orderDescending = trimmedOrderbyClause.EndsWith(" desc");

                var indexOfFirstSpace = trimmedOrderbyClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1
                    ? trimmedOrderbyClause
                    : trimmedOrderbyClause.Remove(indexOfFirstSpace);

                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }

                var propertyMappingValue = mappingDictionary[propertyName];
                if(propertyMappingValue == null)
                    throw new ArgumentNullException("PropertyMappingValue");

                foreach (var desc in propertyMappingValue.DestinationProperties.Reverse())
                {
                    if (propertyMappingValue.Revert)
                        orderDescending = !orderDescending;

                    //source = source.OrderBy(desc + (orderDescending ? " descending" : " ascending"));
                }
            }

            return source;
        }
    }
}
