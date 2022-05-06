using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COVERater.API.Services
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties { get; private set; }
        public bool Revert { get; private set; }

        public PropertyMappingValue(IEnumerable<string> destinationPorperties, bool revert = false)
        {
            DestinationProperties =
                destinationPorperties ?? throw new ArgumentNullException(nameof(destinationPorperties));
            Revert = revert;
        }
    }
}
