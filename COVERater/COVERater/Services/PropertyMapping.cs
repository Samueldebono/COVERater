using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COVERater.API.Services
{
    public class PropertyMapping<TSource, TDestination>:IPropertyMapping
    {
        public Dictionary<string, PropertyMappingValue> _MappingDictionary { get; private set; }

        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            _MappingDictionary = mappingDictionary ??
                                 throw new ArgumentNullException(nameof(mappingDictionary));
        }

    }
}
