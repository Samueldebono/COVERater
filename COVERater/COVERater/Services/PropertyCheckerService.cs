using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace COVERater.API.Services
{
    public interface IPropertyCheckerService
    {
        bool TypeHasProperties<T>(string fields);
    }

    public class PropertyCheckerService : IPropertyCheckerService
    {
        public bool TypeHasProperties<T>(string fields)
        {

            if (string.IsNullOrEmpty(fields)) return true;

            var fieldsAfterSplit = fields.Split(',');

            foreach (var fieldAfterSplit in fieldsAfterSplit)
            {
                var trimmedField = fieldAfterSplit.Trim();
                var propertyInfo =
                    typeof(T).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                
                if (propertyInfo == null) return false;
            }

            return true;
        }
    }
}
